using InvestmentTracker.Domain.DTOs;
using InvestmentTracker.Domain.Interfaces;

namespace InvestmentTracker.Domain.Services;

public class ReturnCalculator : IReturnCalculator
{
    public decimal CalculatePeriodReturn(IEnumerable<PortfolioHistoryPoint> history, DateOnly endDate, DateOnly startDate)
    {
        var historyList = history.ToList();
        var startPoint = historyList.FirstOrDefault(p => p.Date >= startDate);
        var endPoint = historyList.LastOrDefault(p => p.Date <= endDate);

        if (startPoint is null || endPoint is null || startPoint.Value == 0)
            return 0;

        // Simple return: (EndValue - StartValue) / StartValue * 100
        return Math.Round((endPoint.Value - startPoint.Value) / startPoint.Value * 100, 2);
    }
}
