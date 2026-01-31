namespace InvestmentTracker.Api.Features.Assets.ManageContributions;

public record AddContributionRequest(decimal Amount, DateTime DateMade, string? Note);

public record GetContributionsResponse(int Id, int AssetId, decimal Amount, DateTime DateMade, string? Note);
