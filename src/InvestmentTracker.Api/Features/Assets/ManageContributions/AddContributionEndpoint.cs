using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Api.Features.Assets.ManageContributions;

public static class AddContributionEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/assets/{id}/contributions", Handle)
           .WithName("AddAssetContribution")
           .WithTags("Assets")
           .WithSummary("Add a new contribution for an asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id, AddContributionRequest request)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset == null) return Results.NotFound();

        var contribution = new Contribution
        {
            AssetId = id,
            Amount = request.Amount,
            DateMade = request.DateMade,
            Note = request.Note
        };

        db.Contributions.Add(contribution);
        await db.SaveChangesAsync();

        return Results.Created($"/assets/{id}/contributions", null);
    }
}
