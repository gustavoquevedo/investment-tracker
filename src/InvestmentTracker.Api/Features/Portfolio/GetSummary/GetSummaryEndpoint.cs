using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Domain.Interfaces;

namespace InvestmentTracker.Api.Features.Portfolio.GetSummary;

public static class GetSummaryEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/summary", Handle)
           .WithName("GetPortfolioSummary")
           .WithTags("Portfolio")
           .WithSummary("Get portfolio summary with aggregate metrics.");
    }

    private static async Task<IResult> Handle(
        InvestmentContext db,
        IFeeCalculator feeCalculator)
    {
        var assets = await db.Assets.ToListAsync();
        
        decimal totalInvested = 0;
        decimal totalValue = 0;
        decimal totalFees = 0;
        DateTime? lastUpdated = null;

        foreach (var asset in assets)
        {
            var contributions = await db.Contributions
                .Where(c => c.AssetId == asset.Id)
                .ToListAsync();
            totalInvested += contributions.Sum(c => c.Amount);

            var latestSnapshot = await db.Snapshots
                .Where(s => s.AssetId == asset.Id)
                .OrderByDescending(s => s.SnapshotDate)
                .FirstOrDefaultAsync();

            if (latestSnapshot is not null)
            {
                totalValue += latestSnapshot.TotalValue;
                
                if (lastUpdated is null || latestSnapshot.SnapshotDate > lastUpdated)
                {
                    lastUpdated = latestSnapshot.SnapshotDate;
                }
                
                if (contributions.Count > 0 && asset.FeePercentagePerYear > 0)
                {
                    var earliestContributionDate = contributions.Min(c => c.DateMade);
                    var principal = contributions.Sum(c => c.Amount);
                    var fee = feeCalculator.CalculateFee(
                        principal,
                        asset.FeePercentagePerYear,
                        earliestContributionDate,
                        latestSnapshot.SnapshotDate);
                    totalFees += fee;
                }
            }
        }

        var totalPnL = totalValue - totalInvested;
        
        var response = new GetSummaryResponse(
            totalValue,
            totalInvested,
            totalPnL,
            totalInvested > 0 ? Math.Round(totalPnL / totalInvested * 100, 2) : 0,
            assets.Count,
            lastUpdated);

        return Results.Ok(response);
    }
}
