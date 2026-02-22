namespace InvestmentTracker.Api.Features.Portfolio.GetAssetsWithSnapshots;

public record AssetWithSnapshotsDto(
    int Id,
    string Name,
    string AssetType,
    string? Ticker,
    string? Isin,
    decimal FeePercentagePerYear,
    List<SnapshotEntryDto> Snapshots,
    List<ContributionEntryDto> Contributions);

public record SnapshotEntryDto(DateTime SnapshotDate, decimal TotalValue);

public record ContributionEntryDto(DateTime DateMade, decimal Amount);
