using InvestmentTracker.Domain.Enums;

namespace InvestmentTracker.Domain.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // The Enum we defined above
        public AssetType AssetType { get; set; }

        // Metadata (Nullable, because Cash doesn't have an ISIN)
        public string? Isin { get; set; }
        public string? Ticker { get; set; }

        // Stored as decimal for precision (e.g., 0.0050 for 0.50%)
        public decimal FeePercentagePerYear { get; set; }

        // Audit field
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Navigation Properties ---
        // Initialized to empty lists to avoid NullReferenceExceptions
        public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
        public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
        public virtual ICollection<AssetTag> AssetTags { get; set; } = new List<AssetTag>();
    }
}