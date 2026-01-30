using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using InvestmentTracker.Infra.Data;
using InvestmentTracker.Infra.Repositories;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Enums;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace InvestmentTracker.Tests.Infra;

public class RepositoryTests
{
    private InvestmentContext GetContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<InvestmentContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new InvestmentContext(options);
    }

    [Fact]
    public async Task AssetRepository_ShouldAddAndRetrieveAsset()
    {
        // Arrange
        using var context = GetContext("AssetTestDb");
        var repository = new AssetRepository(context);
        var asset = new Asset { Name = "Test Asset", AssetType = AssetType.Stock };

        // Act
        await repository.AddAsync(asset);
        var retrieved = await repository.GetByIdAsync(asset.Id);

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be("Test Asset");
    }

    [Fact]
    public async Task ContributionRepository_ShouldAddAndRetrieveByAsset()
    {
        // Arrange
        using var context = GetContext("ContributionTestDb");
        var assetRepo = new AssetRepository(context);
        var contributionRepo = new ContributionRepository(context);

        var asset = new Asset { Name = "Test", AssetType = AssetType.Cash };
        await assetRepo.AddAsync(asset);

        var contribution = new Contribution { AssetId = asset.Id, Amount = 100, DateMade = DateTime.Now };

        // Act
        await contributionRepo.AddAsync(contribution);
        var result = await contributionRepo.GetByAssetIdAsync(asset.Id);

        // Assert
        result.Should().ContainSingle();
        result.First().Amount.Should().Be(100);
    }

    [Fact]
    public async Task SnapshotRepository_ShouldGetLatestSnapshot()
    {
        // Arrange
        using var context = GetContext("SnapshotTestDb");
        var assetRepo = new AssetRepository(context);
        var snapshotRepo = new SnapshotRepository(context);

        var asset = new Asset { Name = "Test", AssetType = AssetType.Cash };
        await assetRepo.AddAsync(asset);

        var s1 = new Snapshot { AssetId = asset.Id, TotalValue = 100, SnapshotDate = DateTime.Now.AddDays(-2) };
        var s2 = new Snapshot { AssetId = asset.Id, TotalValue = 200, SnapshotDate = DateTime.Now }; // Latest
        var s3 = new Snapshot { AssetId = asset.Id, TotalValue = 150, SnapshotDate = DateTime.Now.AddDays(-1) };

        await snapshotRepo.AddAsync(s1);
        await snapshotRepo.AddAsync(s2);
        await snapshotRepo.AddAsync(s3);

        // Act
        var latest = await snapshotRepo.GetLatestByAssetIdAsync(asset.Id);

        // Assert
        latest.Should().NotBeNull();
        latest!.TotalValue.Should().Be(200);
    }

    [Fact]
    public async Task AssetRepository_ShouldFilterByTag()
    {
        // Arrange
        using var context = GetContext("AssetTagTestDb");
        var repository = new AssetRepository(context);

        var tag1 = new Tag { Name = "Risky" };
        var tag2 = new Tag { Name = "Safe" };
        
        var asset1 = new Asset { Name = "Asset1", AssetType = AssetType.Stock };
        var asset2 = new Asset { Name = "Asset2", AssetType = AssetType.Cash };
        
        context.Tags.AddRange(tag1, tag2);
        context.Assets.AddRange(asset1, asset2);
        await context.SaveChangesAsync();

        // Link tags
        context.AssetTags.Add(new AssetTag { AssetId = asset1.Id, TagId = tag1.Id });
        context.AssetTags.Add(new AssetTag { AssetId = asset2.Id, TagId = tag2.Id });
        await context.SaveChangesAsync();

        // Act - Filter by 'Risky'
        var riskyAssets = await repository.GetAllAsync("Risky");
        var safeAssets = await repository.GetAllAsync("Safe");
        var allAssets = await repository.GetAllAsync();

        // Assert
        riskyAssets.Should().ContainSingle();
        riskyAssets.First().Name.Should().Be("Asset1");
        riskyAssets.First().AssetTags.Should().ContainSingle(); // Verify eager load
        
        safeAssets.Should().ContainSingle();
        safeAssets.First().Name.Should().Be("Asset2");

        allAssets.Should().HaveCount(2);
    }
}
