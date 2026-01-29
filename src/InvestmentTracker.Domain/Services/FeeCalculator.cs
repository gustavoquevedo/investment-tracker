using InvestmentTracker.Domain.Interfaces;

namespace InvestmentTracker.Domain.Services;

public class FeeCalculator : IFeeCalculator
{
    public decimal CalculateFee(decimal principal, decimal annualRate, DateTime startDate, DateTime endDate)
    {
         if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be after end date.");
        }

        var days = (endDate - startDate).Days;
        
        // Simple interest approximation
        // AnnualRate is expected to be a decimal (e.g. 0.01 for 1%)
        // Calculation: (Principal * AnnualRate * Days) / 365
        var fee = (principal * annualRate * days) / 365m;

        return fee;
    }
}
