namespace InvestmentTracker.Domain.DTOs
{
    public class PortfolioSummary
    {
        public decimal TotalInvested { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalPnL { get; set; }
        public decimal TotalFees { get; set; }
        public decimal NetPnL { get; set; }
        public int AssetCount { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class PortfolioHistory
    {
        public List<PortfolioHistoryPoint> Points { get; set; } = [];
    }

    public class PortfolioHistoryPoint
    {
        public DateOnly Date { get; set; }
        public decimal Value { get; set; }
        public decimal Invested { get; set; }
    }

    public class PortfolioAllocation
    {
        public List<AllocationEntry> ByType { get; set; } = [];
    }

    public class AllocationEntry
    {
        public required string Type { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
    }

    public class PortfolioReturns
    {
        public decimal Twr { get; set; }
        public decimal Mwr { get; set; }
        public PeriodReturns Periods { get; set; } = new();
    }

    public class PeriodReturns
    {
        public decimal Ytd { get; set; }
        public decimal OneYear { get; set; }
        public decimal ThreeYear { get; set; }
        public decimal FiveYear { get; set; }
        public decimal AllTime { get; set; }
    }
}
