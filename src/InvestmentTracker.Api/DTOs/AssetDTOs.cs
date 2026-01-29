namespace InvestmentTracker.Api.DTOs
{
    public class AssetResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AssetType { get; set; }
        public string? ISIN { get; set; }
        public string? Ticker { get; set; }
        public decimal FeePercentagePerYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TagDto> Tags { get; set; } = new();
    }

    public class TagDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Color { get; set; }
    }

    public class CreateAssetRequest
    {
        public required string Name { get; set; }
        public required string AssetType { get; set; }
        public string? ISIN { get; set; }
        public string? Ticker { get; set; }
        public decimal FeePercentagePerYear { get; set; }
    }

    public class UpdateAssetRequest
    {
        public string? Name { get; set; }
        public string? AssetType { get; set; }
        public string? ISIN { get; set; }
        public string? Ticker { get; set; }
        public decimal? FeePercentagePerYear { get; set; }
    }

    public class CreateSnapshotRequest
    {
        public decimal TotalValue { get; set; }
        public DateTime SnapshotDate { get; set; }
    }

    public class CreateContributionRequest
    {
        public decimal Amount { get; set; }
        public DateTime DateMade { get; set; }
        public string? Note { get; set; }
    }
}
