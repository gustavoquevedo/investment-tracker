using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Api.Features.Tags.CreateTag;

public static class CreateTagEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/tags", Handle)
           .WithName("CreateTag")
           .WithTags("Tags")
           .WithSummary("Create a new tag.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, CreateTagRequest request)
    {
        var tag = new Tag
        {
            Name = request.Name,
            ColorHex = request.ColorHex
        };

        db.Tags.Add(tag);
        await db.SaveChangesAsync();

        var response = new CreateTagResponse(tag.Id, tag.Name, tag.ColorHex);

        return Results.Created($"/tags/{tag.Id}", response);
    }
}
