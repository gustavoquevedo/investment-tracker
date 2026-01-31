namespace InvestmentTracker.Api.Features.Assets.GetAssets;

public record GetAssetsResponse(
    int Id,
    string Name,
    string AssetType,
    string? ISIN,
    string? Ticker,
    decimal FeePercentagePerYear,
    DateTime CreatedAt,
    List<Common.TagDto> Tags);
