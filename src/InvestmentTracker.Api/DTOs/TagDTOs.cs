namespace InvestmentTracker.Api.DTOs
{
    public class TagResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ColorHex { get; set; }
    }

    public class CreateTagRequest
    {
        public required string Name { get; set; }
        public string? ColorHex { get; set; }
    }
}
