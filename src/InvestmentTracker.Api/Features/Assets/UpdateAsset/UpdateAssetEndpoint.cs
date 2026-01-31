using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Domain.Enums;
using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.UpdateAsset;

public static class UpdateAssetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/assets/{id}", Handle)
           .WithName("UpdateAsset")
           .WithTags("Assets")
           .WithSummary("Update an existing asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id, UpdateAssetRequest request)
    {
        var asset = await db.Assets
            .Include(a => a.AssetTags)
            .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset == null) return Results.NotFound();

        if (request.Name != null) asset.Name = request.Name;
        if (request.ISIN != null) asset.Isin = request.ISIN;
        if (request.Ticker != null) asset.Ticker = request.Ticker;
        if (request.FeePercentagePerYear.HasValue) asset.FeePercentagePerYear = request.FeePercentagePerYear.Value;
        
        if (request.AssetType != null)
        {
             if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
            {
                return Results.BadRequest($"Invalid AssetType. Valid values: {string.Join(", ", Enum.GetNames(typeof(AssetType)))}");
            }
            asset.AssetType = assetType;
        }

        await db.SaveChangesAsync();

        var response = new UpdateAssetResponse(
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
