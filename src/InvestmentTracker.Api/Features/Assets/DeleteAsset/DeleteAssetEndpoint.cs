using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InvestmentTracker.Api.Features.Assets.DeleteAsset;

public static class DeleteAssetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/assets/{id}", Handle)
           .WithName("DeleteAsset")
           .WithTags("Assets")
           .WithSummary("Delete an asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset == null) return Results.NotFound();

        db.Assets.Remove(asset);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
