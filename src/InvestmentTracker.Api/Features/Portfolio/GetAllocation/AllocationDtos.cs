namespace InvestmentTracker.Api.Features.Portfolio.GetAllocation;

public record GetAllocationResponse(List<AllocationEntry> ByType);
public record AllocationEntry(string Type, decimal Value, decimal Percentage);
