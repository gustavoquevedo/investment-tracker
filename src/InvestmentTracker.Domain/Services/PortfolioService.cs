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
        
        decimal totalInvested = 0;
        decimal totalValue = 0;
        decimal totalFees = 0;

        foreach (var asset in assets)
        {
            var contributions = await _contributionRepository.GetByAssetIdAsync(asset.Id);
            var contributionsList = contributions.ToList();
            totalInvested += contributionsList.Sum(c => c.Amount);

            var latestSnapshot = await _snapshotRepository.GetLatestByAssetIdAsync(asset.Id);
            if (latestSnapshot != null)
            {
                totalValue += latestSnapshot.TotalValue;
                
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
            NetPnL = totalPnL - totalFees
        };
    }
}
