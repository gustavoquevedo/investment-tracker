using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Api.Features.Portfolio.GetSummary;
using InvestmentTracker.Api.Features.Portfolio.GetHistory;
using InvestmentTracker.Api.Features.Portfolio.GetAllocation;
using InvestmentTracker.Api.Features.Portfolio.GetReturns;
using InvestmentTracker.Api.Features.Portfolio.GetAssetsWithSnapshots;

namespace InvestmentTracker.Api.Features.Portfolio;

public static class PortfolioEndpoints
{
    public static void MapPortfolioEndpoints(this IEndpointRouteBuilder app)
    {
        GetSummaryEndpoint.Map(app);
        GetHistoryEndpoint.Map(app);
        GetAllocationEndpoint.Map(app);
        GetReturnsEndpoint.Map(app);
        GetAssetsWithSnapshotsEndpoint.Map(app);
    }
}
