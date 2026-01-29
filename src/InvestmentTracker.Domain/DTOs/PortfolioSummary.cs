namespace InvestmentTracker.Domain.DTOs
{
    public class PortfolioSummary
    {
        public decimal TotalInvested { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalPnL { get; set; }
    }
}
