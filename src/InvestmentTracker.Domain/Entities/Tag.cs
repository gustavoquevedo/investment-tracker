namespace InvestmentTracker.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ColorHex { get; set; } // e.g. "#FF5733"

        // Navigation property for the Many-to-Many link
        public virtual ICollection<AssetTag> AssetTags { get; set; } = new List<AssetTag>();
    }
}