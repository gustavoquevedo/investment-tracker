using InvestmentTracker.Domain.DTOs;

namespace InvestmentTracker.Domain.Interfaces;

public interface IReturnCalculator
{
    decimal CalculatePeriodReturn(IEnumerable<PortfolioHistoryPoint> history, DateOnly endDate, DateOnly startDate);
}
