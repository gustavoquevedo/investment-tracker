using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Tags.GetTags;

public static class GetTagsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/tags", Handle)
           .WithName("GetTags")
           .WithTags("Tags")
           .WithSummary("List all available tags.");
    }

    private static async Task<IResult> Handle(InvestmentContext db)
    {
        var tags = await db.Tags.ToListAsync();
        
        var response = tags.Select(t => new GetTagsResponse(t.Id, t.Name, t.ColorHex));

        return Results.Ok(response);
    }
}
