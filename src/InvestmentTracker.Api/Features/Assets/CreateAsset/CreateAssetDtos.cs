using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.CreateAsset;

public record CreateAssetRequest(
    string Name,
    string AssetType,
    string? ISIN,
    string? Ticker,
    decimal FeePercentagePerYear);

public record CreateAssetResponse(
    int Id,
    string Name,
    string AssetType,
    string? ISIN,
    string? Ticker,
    decimal FeePercentagePerYear,
    DateTime CreatedAt,
    List<TagDto> Tags);
