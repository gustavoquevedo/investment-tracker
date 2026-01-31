namespace InvestmentTracker.Api.Features.Assets.ManageSnapshots;

public record AddSnapshotRequest(decimal TotalValue, DateTime SnapshotDate);

public record GetSnapshotsResponse(int Id, int AssetId, decimal TotalValue, DateTime SnapshotDate);
