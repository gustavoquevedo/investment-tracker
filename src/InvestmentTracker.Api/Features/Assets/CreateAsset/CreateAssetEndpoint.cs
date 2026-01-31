using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Enums;
using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.CreateAsset;

public static class CreateAssetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/assets", Handle)
           .WithName("CreateAsset")
           .WithTags("Assets")
           .WithSummary("Create a new asset.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, CreateAssetRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest("Asset name cannot be empty.");
        }

        if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
        {
            return Results.BadRequest($"Invalid AssetType. Valid values: {string.Join(", ", Enum.GetNames(typeof(AssetType)))}");
        }

        var asset = new Asset
        {
            Name = request.Name,
            AssetType = assetType,
            Isin = request.ISIN,
            Ticker = request.Ticker,
            FeePercentagePerYear = request.FeePercentagePerYear
        };

        db.Assets.Add(asset);
        await db.SaveChangesAsync();

        var response = new CreateAssetResponse(
            asset.Id,
            asset.Name,
            asset.AssetType.ToString(),
            asset.Isin,
            asset.Ticker,
            asset.FeePercentagePerYear,
            asset.CreatedAt,
            new List<TagDto>()
        );

        return Results.Created($"/assets/{asset.Id}", response);
    }
}
