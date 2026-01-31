namespace InvestmentTracker.Api.Features.Portfolio.GetHistory;

public record GetHistoryResponse(List<HistoryPoint> Points);
public record HistoryPoint(DateOnly Date, decimal Value, decimal Invested);
