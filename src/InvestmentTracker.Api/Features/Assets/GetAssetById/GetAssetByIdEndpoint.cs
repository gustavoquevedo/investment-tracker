using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.GetAssetById;

public static class GetAssetByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/assets/{id}", Handle)
           .WithName("GetAssetById")
           .WithTags("Assets")
           .WithSummary("Get a single asset by ID.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id)
    {
        var asset = await db.Assets
            .Include(a => a.AssetTags)
            .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset == null) return Results.NotFound();
        
        var response = new GetAssetByIdResponse(
            asset.Id,
            asset.Name,
            asset.AssetType.ToString(),
            asset.Isin,
            asset.Ticker,
            asset.FeePercentagePerYear,
            asset.CreatedAt,
            asset.AssetTags.Select(at => new TagDto(at.Tag.Id, at.Tag.Name, at.Tag.ColorHex)).ToList()
        );

        return Results.Ok(response);
    }
}
