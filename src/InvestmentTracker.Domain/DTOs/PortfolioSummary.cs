namespace InvestmentTracker.Domain.DTOs
{
    public class PortfolioSummary
    {
        public decimal TotalInvested { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalPnL { get; set; }
        public decimal TotalFees { get; set; }
        public decimal NetPnL { get; set; }
    }
}
