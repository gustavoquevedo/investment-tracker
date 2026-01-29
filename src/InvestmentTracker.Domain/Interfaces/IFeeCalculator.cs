namespace InvestmentTracker.Domain.Interfaces;

public interface IFeeCalculator
{
    decimal CalculateFee(decimal principal, decimal annualRate, DateTime startDate, DateTime endDate);
}
