using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Portfolio.GetAssetsWithSnapshots;

public static class GetAssetsWithSnapshotsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/assets-with-snapshots", Handle)
           .WithName("GetAssetsWithSnapshots")
           .WithTags("Portfolio")
           .WithSummary("Get all assets with their full snapshot history and contributions.");
    }

    private static async Task<IResult> Handle(InvestmentContext db)
    {
        var assets = await db.Assets
            .Include(a => a.Snapshots)
            .Include(a => a.Contributions)
            .ToListAsync();

        var result = assets.Select(a => new AssetWithSnapshotsDto(
            a.Id,
            a.Name,
            a.AssetType.ToString(),
            a.Ticker,
            a.Isin,
            a.FeePercentagePerYear,
            a.Snapshots
                .OrderByDescending(s => s.SnapshotDate)
                .Select(s => new SnapshotEntryDto(s.SnapshotDate, s.TotalValue))
                .ToList(),
            a.Contributions
                .OrderBy(c => c.DateMade)
                .Select(c => new ContributionEntryDto(c.DateMade, c.Amount))
                .ToList()
        )).ToList();

        return Results.Ok(result);
    }
}
