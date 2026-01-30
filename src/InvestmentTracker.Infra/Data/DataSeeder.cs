using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Infra.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(InvestmentContext context)
    {
        // Only seed if database is empty
        if (await context.Assets.AnyAsync())
        {
            return;
        }

        // 1. Create Assets
        var sp500 = new Asset
        {
            Name = "Vanguard S&P 500 ETF",
            Ticker = "VOO",
            Isin = "US9229087696",
            AssetType = AssetType.Stock,
            FeePercentagePerYear = 0.0003m, // 0.03%
            CreatedAt = DateTime.UtcNow
        };

        var btc = new Asset
        {
            Name = "Bitcoin Cold Storage",
            Ticker = "BTC",
            AssetType = AssetType.Crypto,
            FeePercentagePerYear = 0m,
            CreatedAt = DateTime.UtcNow
        };

        var bondFund = new Asset
        {
            Name = "Total Bond Market ETF",
            Ticker = "BND",
            Isin = "US9219378356",
            AssetType = AssetType.Bond,
            FeePercentagePerYear = 0.0003m,
            CreatedAt = DateTime.UtcNow
        };

        context.Assets.AddRange(sp500, btc, bondFund);
        await context.SaveChangesAsync();

        // 2. Create Tags
        var tagRetirement = new Tag { Name = "Retirement", ColorHex = "#4caf50" }; // Green
        var tagRisky = new Tag { Name = "Risky", ColorHex = "#f44336" }; // Red
        var tagSafe = new Tag { Name = "Safe", ColorHex = "#2196f3" }; // Blue

        context.Tags.AddRange(tagRetirement, tagRisky, tagSafe);
        await context.SaveChangesAsync();

        // 3. Link Tags
        context.AssetTags.AddRange(
            new AssetTag { AssetId = sp500.Id, TagId = tagRetirement.Id },
            new AssetTag { AssetId = bondFund.Id, TagId = tagSafe.Id },
            new AssetTag { AssetId = btc.Id, TagId = tagRisky.Id }
        );
        await context.SaveChangesAsync();

        // 4. Generate History (Contributions & Snapshots)
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startDate = today.AddYears(-1); // 12 months history

        // Helper to simulate data
        await SimulateAssetHistory(context, sp500, startDate, 10000m, 1000m, 0.10m); // Start 10k, Add 1k/mo, 10% annual return
        await SimulateAssetHistory(context, bondFund, startDate, 5000m, 500m, 0.04m); // Start 5k, Add 500/mo, 4% annual return
        
        // BTC started 6 months ago, high volatility
        await SimulateAssetHistory(context, btc, today.AddMonths(-6), 1000m, 100m, 0.50m, volatility: 0.20m); 

        await context.SaveChangesAsync();
    }

    private static async Task SimulateAssetHistory(
        InvestmentContext context, 
        Asset asset, 
        DateOnly start, 
        decimal initialAmount, 
        decimal monthlyContribution, 
        decimal annualReturn,
        decimal volatility = 0.02m)
    {
        var random = new Random(asset.Id); // Deterministic randomness per asset
        decimal currentInvested = 0;
        decimal currentUnits = 0; // tracking "units" abstractly to calculate value
        decimal unitPrice = 100m; // Arbitrary start price

        var months = (DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - start.DayNumber) / 30;
        
        DateTime currentDate = start.ToDateTime(TimeOnly.MinValue);

        // Initial Contribution
        context.Contributions.Add(new Contribution
        {
            AssetId = asset.Id,
            Amount = initialAmount,
            DateMade = currentDate,
            Note = "Initial Deposit"
        });
        currentInvested += initialAmount;
        currentUnits += initialAmount / unitPrice;

        // Current snapshot after initial deposit
        context.Snapshots.Add(new Snapshot
        {
            AssetId = asset.Id,
            TotalValue = currentUnits * unitPrice,
            SnapshotDate = currentDate
        });

        // Monthly progression
        for (int i = 1; i <= months; i++)
        {
            currentDate = currentDate.AddMonths(1);

            // Price Fluctuation
            var monthlyReturn = annualReturn / 12;
            var fluctuation = (decimal)(random.NextDouble() * 2 - 1) * volatility;
            unitPrice *= (1 + monthlyReturn + fluctuation);

            // Contribution
            context.Contributions.Add(new Contribution
            {
                AssetId = asset.Id,
                Amount = monthlyContribution,
                DateMade = currentDate,
                Note = "Monthly Savings"
            });
            currentInvested += monthlyContribution;
            currentUnits += monthlyContribution / unitPrice;

            // Snapshot
            context.Snapshots.Add(new Snapshot
            {
                AssetId = asset.Id,
                TotalValue = currentUnits * unitPrice,
                SnapshotDate = currentDate
            });
        }
    }
}
