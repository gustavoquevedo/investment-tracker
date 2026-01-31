namespace InvestmentTracker.Api.Features.Portfolio.GetReturns;

public record GetReturnsResponse(decimal Twr, decimal Mwr, PeriodReturns Periods);
public record PeriodReturns(decimal Ytd, decimal OneYear, decimal ThreeYear, decimal FiveYear, decimal AllTime);
