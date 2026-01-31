using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Portfolio.GetAllocation;

public static class GetAllocationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/allocation", Handle)
           .WithName("GetPortfolioAllocation")
           .WithTags("Portfolio")
           .WithSummary("Get asset allocation breakdown by type.");
    }

    private static async Task<IResult> Handle(InvestmentContext db)
    {
        var assets = await db.Assets.ToListAsync();
        var allocationByType = new Dictionary<string, decimal>();
        decimal totalValue = 0;

        foreach (var asset in assets)
        {
            var latestSnapshot = await db.Snapshots
                .Where(s => s.AssetId == asset.Id)
                .OrderByDescending(s => s.SnapshotDate)
                .FirstOrDefaultAsync();
                
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
            .Select(kvp => new AllocationEntry(
                kvp.Key, 
                kvp.Value, 
                totalValue > 0 ? Math.Round(kvp.Value / totalValue * 100, 2) : 0))
            .OrderByDescending(e => e.Value)
            .ToList();

        return Results.Ok(new GetAllocationResponse(entries));
    }
}
