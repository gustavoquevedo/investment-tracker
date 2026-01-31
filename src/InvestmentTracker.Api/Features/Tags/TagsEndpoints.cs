using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Api.Features.Tags.GetTags;
using InvestmentTracker.Api.Features.Tags.CreateTag;
using InvestmentTracker.Api.Features.Tags.DeleteTag;

namespace InvestmentTracker.Api.Features.Tags;

public static class TagsEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        GetTagsEndpoint.Map(app);
        CreateTagEndpoint.Map(app);
        DeleteTagEndpoint.Map(app);
    }
}
