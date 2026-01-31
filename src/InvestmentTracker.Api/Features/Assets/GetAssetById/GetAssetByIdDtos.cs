using InvestmentTracker.Api.Common;

namespace InvestmentTracker.Api.Features.Assets.GetAssetById;

public record GetAssetByIdResponse(
    int Id,
    string Name,
    string AssetType,
    string? ISIN,
    string? Ticker,
    decimal FeePercentagePerYear,
    DateTime CreatedAt,
    List<TagDto> Tags);
