using InvestmentTracker.Domain.DTOs;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Interfaces;

namespace InvestmentTracker.Domain.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IAssetRepository _assetRepository;
    private readonly IContributionRepository _contributionRepository;
    private readonly ISnapshotRepository _snapshotRepository;
    private readonly IFeeCalculator _feeCalculator;

    public PortfolioService(
        IAssetRepository assetRepository,
        IContributionRepository contributionRepository,
        ISnapshotRepository snapshotRepository,
        IFeeCalculator feeCalculator)
    {
        _assetRepository = assetRepository;
        _contributionRepository = contributionRepository;
        _snapshotRepository = snapshotRepository;
        _feeCalculator = feeCalculator;
    }

    public async Task<Asset> CreateAssetAsync(Asset asset)
    {
            if (string.IsNullOrWhiteSpace(asset.Name))
        {
            throw new ArgumentException("Asset name cannot be empty.");
        }
        return await _assetRepository.AddAsync(asset);
    }

    public async Task AddContributionAsync(int assetId, Contribution contribution)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId);
        if (asset == null)
        {
            throw new KeyNotFoundException($"Asset with ID {assetId} not found.");
        }
        
        // Ensure consistency
        contribution.AssetId = assetId; 
        await _contributionRepository.AddAsync(contribution);
    }

    public async Task AddSnapshotAsync(int assetId, Snapshot snapshot)
    {
            var asset = await _assetRepository.GetByIdAsync(assetId);
        if (asset == null)
        {
            throw new KeyNotFoundException($"Asset with ID {assetId} not found.");
        }
        
        snapshot.AssetId = assetId;
        await _snapshotRepository.AddAsync(snapshot);
    }

    public async Task<PortfolioSummary> GetPortfolioSummaryAsync()
    {
        var assets = await _assetRepository.GetAllAsync();
        var assetList = assets.ToList();
        
        decimal totalInvested = 0;
        decimal totalValue = 0;
        decimal totalFees = 0;
        DateTime? lastUpdated = null;

        foreach (var asset in assetList)
        {
            var contributions = await _contributionRepository.GetByAssetIdAsync(asset.Id);
            var contributionsList = contributions.ToList();
            totalInvested += contributionsList.Sum(c => c.Amount);

            var latestSnapshot = await _snapshotRepository.GetLatestByAssetIdAsync(asset.Id);
            if (latestSnapshot is not null)
            {
                totalValue += latestSnapshot.TotalValue;
                
                if (lastUpdated is null || latestSnapshot.SnapshotDate > lastUpdated)
                {
                    lastUpdated = latestSnapshot.SnapshotDate;
                }
                
                // Calculate fees if we have contributions and a snapshot
                if (contributionsList.Count > 0 && asset.FeePercentagePerYear > 0)
                {
                    var earliestContributionDate = contributionsList.Min(c => c.DateMade);
                    var principal = contributionsList.Sum(c => c.Amount);
                    var fee = _feeCalculator.CalculateFee(
                        principal,
                        asset.FeePercentagePerYear,
                        earliestContributionDate,
                        latestSnapshot.SnapshotDate);
                    totalFees += fee;
                }
            }
        }

        var totalPnL = totalValue - totalInvested;
        
        return new PortfolioSummary
        {
            TotalInvested = totalInvested,
            TotalValue = totalValue,
            TotalPnL = totalPnL,
            TotalFees = totalFees,
            NetPnL = totalPnL - totalFees,
            AssetCount = assetList.Count,
            LastUpdated = lastUpdated
        };
    }

    public async Task<PortfolioHistory> GetHistoryAsync(DateOnly? from, DateOnly? to)
    {
        var assets = await _assetRepository.GetAllAsync();
        var snapshotsByDate = new Dictionary<DateOnly, (decimal Value, decimal Invested)>();

        // Default: last 5 years if no range specified
        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        foreach (var asset in assets)
        {
            var snapshots = await _snapshotRepository.GetByAssetIdAsync(asset.Id);
            var contributions = await _contributionRepository.GetByAssetIdAsync(asset.Id);
            var contributionsList = contributions.ToList();

            foreach (var snapshot in snapshots)
            {
                var snapshotDate = DateOnly.FromDateTime(snapshot.SnapshotDate);
                if (snapshotDate < fromDate || snapshotDate > toDate)
                    continue;

                // Calculate invested amount up to this snapshot date
                var investedToDate = contributionsList
                    .Where(c => DateOnly.FromDateTime(c.DateMade) <= snapshotDate)
                    .Sum(c => c.Amount);

                if (snapshotsByDate.TryGetValue(snapshotDate, out var existing))
                {
                    snapshotsByDate[snapshotDate] = (
                        existing.Value + snapshot.TotalValue,
                        existing.Invested + investedToDate);
                }
                else
                {
                    snapshotsByDate[snapshotDate] = (snapshot.TotalValue, investedToDate);
                }
            }
        }

        var points = snapshotsByDate
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new PortfolioHistoryPoint
            {
                Date = kvp.Key,
                Value = kvp.Value.Value,
                Invested = kvp.Value.Invested
            })
            .ToList();

        return new PortfolioHistory { Points = points };
    }

    public async Task<PortfolioAllocation> GetAllocationAsync()
    {
        var assets = await _assetRepository.GetAllAsync();
        var allocationByType = new Dictionary<string, decimal>();
        decimal totalValue = 0;

        foreach (var asset in assets)
        {
            var latestSnapshot = await _snapshotRepository.GetLatestByAssetIdAsync(asset.Id);
            if (latestSnapshot is null)
                continue;

            var typeName = asset.AssetType.ToString();
            totalValue += latestSnapshot.TotalValue;

            if (allocationByType.TryGetValue(typeName, out var existing))
            {
                allocationByType[typeName] = existing + latestSnapshot.TotalValue;
            }
            else
            {
                allocationByType[typeName] = latestSnapshot.TotalValue;
            }
        }

        var entries = allocationByType
            .Select(kvp => new AllocationEntry
            {
                Type = kvp.Key,
                Value = kvp.Value,
                Percentage = totalValue > 0 ? Math.Round(kvp.Value / totalValue * 100, 2) : 0
            })
            .OrderByDescending(e => e.Value)
            .ToList();

        return new PortfolioAllocation { ByType = entries };
    }

    public async Task<PortfolioReturns> GetReturnsAsync(DateOnly? asOf)
    {
        var referenceDate = asOf ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var summary = await GetPortfolioSummaryAsync();

        // Simple return calculation for now
        // TWR = (EndValue - Contributions) / StartValue
        // For a more accurate TWR, we'd need to track sub-period returns
        
        decimal twr = 0;
        decimal mwr = 0;

        if (summary.TotalInvested > 0)
        {
            // Simple approximation: total return percentage
            twr = Math.Round(summary.TotalPnL / summary.TotalInvested * 100, 2);
            mwr = twr; // Simplified: use same as TWR for now
        }

        // Calculate period returns (simplified - using proportional allocation)
        var history = await GetHistoryAsync(null, null);
        var allTimeReturn = twr;

        var periods = new PeriodReturns
        {
            AllTime = allTimeReturn,
            Ytd = CalculatePeriodReturn(history, referenceDate, new DateOnly(referenceDate.Year, 1, 1)),
            OneYear = CalculatePeriodReturn(history, referenceDate, referenceDate.AddYears(-1)),
            ThreeYear = CalculatePeriodReturn(history, referenceDate, referenceDate.AddYears(-3)),
            FiveYear = CalculatePeriodReturn(history, referenceDate, referenceDate.AddYears(-5))
        };

        return new PortfolioReturns
        {
            Twr = twr,
            Mwr = mwr,
            Periods = periods
        };
    }

    private static decimal CalculatePeriodReturn(PortfolioHistory history, DateOnly endDate, DateOnly startDate)
    {
        var startPoint = history.Points.FirstOrDefault(p => p.Date >= startDate);
        var endPoint = history.Points.LastOrDefault(p => p.Date <= endDate);

        if (startPoint is null || endPoint is null || startPoint.Value == 0)
            return 0;

        // Simple return: (EndValue - StartValue) / StartValue * 100
        return Math.Round((endPoint.Value - startPoint.Value) / startPoint.Value * 100, 2);
    }
}

