using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.UpdateAsset;

public record UpdateAssetRequest(
    string? Name,
    string? AssetType,
    string? ISIN,
    string? Ticker,
    decimal? FeePercentagePerYear);

public record UpdateAssetResponse(
    int Id,
    string Name,
    string AssetType,
    string? ISIN,
    string? Ticker,
    decimal FeePercentagePerYear,
    DateTime CreatedAt,
    List<TagDto> Tags);
