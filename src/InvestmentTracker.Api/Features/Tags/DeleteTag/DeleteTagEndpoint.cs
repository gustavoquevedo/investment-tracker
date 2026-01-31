using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InvestmentTracker.Api.Features.Tags.DeleteTag;

public static class DeleteTagEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tags/{id}", Handle)
           .WithName("DeleteTag")
           .WithTags("Tags")
           .WithSummary("Delete a tag.");
    }

    private static async Task<IResult> Handle(InvestmentContext db, int id)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null) return Results.NotFound();

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
