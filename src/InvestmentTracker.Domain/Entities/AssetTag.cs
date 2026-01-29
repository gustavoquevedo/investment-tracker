namespace InvestmentTracker.Domain.Entities
{
    public class AssetTag
    {
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; } = null!;

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;
    }
}