using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Assets.ManageContributions;

public static class GetContributionsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/assets/{id}/contributions", Handle)
           .WithName("GetAssetContributions")
           .WithTags("Assets")
           .WithSummary("Get contributions for a specific asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset == null) return Results.NotFound();

        var contributions = await db.Contributions
            .Where(c => c.AssetId == id)
            .OrderByDescending(c => c.DateMade)
            .ToListAsync();

        var response = contributions.Select(c => new GetContributionsResponse(
            c.Id, c.AssetId, c.Amount, c.DateMade, c.Note
        ));

        return Results.Ok(response);
    }
}
