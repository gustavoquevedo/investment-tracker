using InvestmentTracker.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Api.Features.Portfolio.GetHistory;

public static class GetHistoryEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/history", Handle)
           .WithName("GetPortfolioHistory")
           .WithTags("Portfolio")
           .WithSummary("Get portfolio value history for charting.");
    }

    private static async Task<IResult> Handle(
        InvestmentContext db,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var assets = await db.Assets.ToListAsync();
        var snapshotsByDate = new Dictionary<DateOnly, (decimal Value, decimal Invested)>();

        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        foreach (var asset in assets)
        {
            var snapshots = await db.Snapshots
                .Where(s => s.AssetId == asset.Id)
                .ToListAsync();
            
            var contributions = await db.Contributions
                .Where(c => c.AssetId == asset.Id)
                .ToListAsync();

            foreach (var snapshot in snapshots)
            {
                var snapshotDate = DateOnly.FromDateTime(snapshot.SnapshotDate);
                if (snapshotDate < fromDate || snapshotDate > toDate)
                    continue;

                var investedToDate = contributions
                    .Where(c => DateOnly.FromDateTime(c.DateMade) <= snapshotDate)
                    .Sum(c => c.Amount);

                if (snapshotsByDate.TryGetValue(snapshotDate, out var existing))
                {
                    snapshotsByDate[snapshotDate] = (
                        existing.Value + snapshot.TotalValue,
                        existing.Invested + investedToDate);
                }
                else
                {
                    snapshotsByDate[snapshotDate] = (snapshot.TotalValue, investedToDate);
                }
            }
        }

        var points = snapshotsByDate
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new HistoryPoint(kvp.Key, kvp.Value.Value, kvp.Value.Invested))
            .ToList();

        return Results.Ok(new GetHistoryResponse(points));
    }
}
