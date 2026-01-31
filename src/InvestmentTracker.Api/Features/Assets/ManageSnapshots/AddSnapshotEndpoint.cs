using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Api.Features.Assets.ManageSnapshots;

public static class AddSnapshotEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/assets/{id}/snapshots", Handle)
           .WithName("AddAssetSnapshot")
           .WithTags("Assets")
           .WithSummary("Add a new value snapshot for an asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id, AddSnapshotRequest request)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset == null) return Results.NotFound();

        var snapshot = new Snapshot 
        { 
            AssetId = id, 
            TotalValue = request.TotalValue, 
            SnapshotDate = request.SnapshotDate 
        };
        
        db.Snapshots.Add(snapshot);
        await db.SaveChangesAsync();

        return Results.Created($"/assets/{id}/snapshots", null);
    }
}
