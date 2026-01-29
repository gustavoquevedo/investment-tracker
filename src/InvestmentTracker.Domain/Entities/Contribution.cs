namespace InvestmentTracker.Domain.Entities
{
    public class Contribution
    {
        public int Id { get; set; }
        
        public int AssetId { get; set; }
        
        // Amount of money added (Deposit)
        public decimal Amount { get; set; }
        
        public DateTime DateMade { get; set; }
        
        public string? Note { get; set; }

        public virtual Asset Asset { get; set; } = null!;
    }
}