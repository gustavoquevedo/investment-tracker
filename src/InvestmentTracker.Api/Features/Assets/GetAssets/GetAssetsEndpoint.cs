using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.GetAssets;

public static class GetAssetsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/assets", Handle)
           .WithName("GetAssets")
           .WithTags("Assets")
           .WithSummary("List all assets, optionally filtered by tag.");
    }

    private static async Task<IResult> Handle(
        InvestmentContext db,
        [FromQuery] string? tag = null)
    {
        var query = db.Assets
            .Include(a => a.AssetTags)
            .ThenInclude(at => at.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(a => a.AssetTags.Any(at => at.Tag.Name == tag));
        }

        var assets = await query.ToListAsync();
        
        var response = assets.Select(a => new GetAssetsResponse(
            a.Id,
            a.Name,
            a.AssetType.ToString(),
            a.Isin,
            a.Ticker,
            a.FeePercentagePerYear,
            a.CreatedAt,
            a.AssetTags.Select(at => new TagDto(at.Tag.Id, at.Tag.Name, at.Tag.ColorHex)).ToList()
        ));

        return Results.Ok(response);
    }
}
