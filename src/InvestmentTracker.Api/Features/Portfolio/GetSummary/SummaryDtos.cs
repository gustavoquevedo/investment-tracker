namespace InvestmentTracker.Api.Features.Portfolio.GetSummary;

public record GetSummaryResponse(
    decimal TotalValue,
    decimal TotalInvested,
    decimal PnL,
    decimal PnLPercent,
    int AssetCount,
    DateTime? LastUpdated);
