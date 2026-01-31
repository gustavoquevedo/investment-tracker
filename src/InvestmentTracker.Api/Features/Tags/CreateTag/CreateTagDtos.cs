namespace InvestmentTracker.Api.Features.Tags.CreateTag;

public record CreateTagRequest(string Name, string? ColorHex);
public record CreateTagResponse(int Id, string Name, string? ColorHex);
