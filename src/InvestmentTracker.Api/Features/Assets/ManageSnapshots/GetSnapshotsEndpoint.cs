using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Assets.ManageSnapshots;

public static class GetSnapshotsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/assets/{id}/snapshots", Handle)
           .WithName("GetAssetSnapshots")
           .WithTags("Assets")
           .WithSummary("Get value snapshots for a specific asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset == null) return Results.NotFound();

        var snapshots = await db.Snapshots
            .Where(s => s.AssetId == id)
            .OrderByDescending(s => s.SnapshotDate)
            .ToListAsync();

        var response = snapshots.Select(s => new GetSnapshotsResponse(
            s.Id, s.AssetId, s.TotalValue, s.SnapshotDate
        ));

        return Results.Ok(response);
    }
}
