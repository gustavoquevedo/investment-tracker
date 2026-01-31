using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Domain.DTOs;

namespace InvestmentTracker.Api.Features.Portfolio.GetReturns;

public static class GetReturnsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/returns", Handle)
           .WithName("GetPortfolioReturns")
           .WithTags("Portfolio")
           .WithSummary("Get calculated portfolio returns.");
    }

    private static async Task<IResult> Handle(
        InvestmentContext db,
        IReturnCalculator returnCalculator,
        [FromQuery] DateOnly? asOf = null)
    {
        var referenceDate = asOf ?? DateOnly.FromDateTime(DateTime.UtcNow);
        
        var assets = await db.Assets.ToListAsync();
        decimal totalInvested = 0;
        decimal totalValue = 0;

        foreach (var asset in assets)
        {
            var latestSnapshot = await db.Snapshots
                .Where(s => s.AssetId == asset.Id)
                .OrderByDescending(s => s.SnapshotDate)
                .FirstOrDefaultAsync();
            
            if (latestSnapshot != null) totalValue += latestSnapshot.TotalValue;

            totalInvested += await db.Contributions
                .Where(c => c.AssetId == asset.Id)
                .SumAsync(c => c.Amount);
        }

        decimal totalPnL = totalValue - totalInvested;
        decimal twr = totalInvested > 0 ? Math.Round(totalPnL / totalInvested * 100, 2) : 0;
        decimal mwr = twr;

        var historyPoints = await GetHistoryData(db);
        
        var periods = new PeriodReturns(
            returnCalculator.CalculatePeriodReturn(historyPoints, referenceDate, new DateOnly(referenceDate.Year, 1, 1)),
            returnCalculator.CalculatePeriodReturn(historyPoints, referenceDate, referenceDate.AddYears(-1)),
            returnCalculator.CalculatePeriodReturn(historyPoints, referenceDate, referenceDate.AddYears(-3)),
            returnCalculator.CalculatePeriodReturn(historyPoints, referenceDate, referenceDate.AddYears(-5)),
            twr
        );

        return Results.Ok(new GetReturnsResponse(twr, mwr, periods));
    }

    private static async Task<List<PortfolioHistoryPoint>> GetHistoryData(InvestmentContext db)
    {
        var assets = await db.Assets.ToListAsync();
        var snapshotsByDate = new Dictionary<DateOnly, (decimal Value, decimal Invested)>();

        foreach (var asset in assets)
        {
            var snapshots = await db.Snapshots.Where(s => s.AssetId == asset.Id).ToListAsync();
            var contributions = await db.Contributions.Where(c => c.AssetId == asset.Id).ToListAsync();

            foreach (var snapshot in snapshots)
            {
                var snapshotDate = DateOnly.FromDateTime(snapshot.SnapshotDate);
                var investedToDate = contributions
                    .Where(c => DateOnly.FromDateTime(c.DateMade) <= snapshotDate)
                    .Sum(c => c.Amount);

                if (snapshotsByDate.TryGetValue(snapshotDate, out var existing))
                {
                    snapshotsByDate[snapshotDate] = (existing.Value + snapshot.TotalValue, existing.Invested + investedToDate);
                }
                else
                {
                    snapshotsByDate[snapshotDate] = (snapshot.TotalValue, investedToDate);
                }
            }
        }

        return snapshotsByDate
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new PortfolioHistoryPoint { Date = kvp.Key, Value = kvp.Value.Value, Invested = kvp.Value.Invested })
            .ToList();
    }
}
