namespace InvestmentTracker.Domain.Entities
{
    public class Snapshot
    {
        public int Id { get; set; }
        
        // Foreign Key
        public int AssetId { get; set; }
        
        // The total market value at this specific moment
        public decimal TotalValue { get; set; }
        
        public DateTime SnapshotDate { get; set; }

        // Navigation back to Parent
        public virtual Asset Asset { get; set; } = null!;
    }
}