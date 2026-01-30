using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using InvestmentTracker.Api.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InvestmentTracker.Api.Tests;

public class PortfolioControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PortfolioControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async ValueTask InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InvestmentTracker.Infra.Data.InvestmentContext>();
        
        db.Assets.RemoveRange(db.Assets);
        await db.SaveChangesAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;


    #region GET /portfolio/summary

    [Fact]
    public async Task GetSummary_ReturnsOk_WhenEmpty()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<PortfolioSummaryDto>(_jsonOptions);
        summary.Should().NotBeNull();
        summary!.TotalValue.Should().Be(0);
        summary.TotalInvested.Should().Be(0);
        summary.AssetCount.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetSummary_ReturnsCalculatedValues_WithData()
    {
        // Arrange - Create asset with contribution and snapshot
        var assetRequest = new CreateAssetRequest { Name = "Summary Test ETF", AssetType = "ETF" };
        var assetResponse = await _client.PostAsJsonAsync("/assets", assetRequest);
        var asset = await assetResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        await _client.PostAsJsonAsync($"/assets/{asset!.Id}/contributions", new CreateContributionRequest
        {
            Amount = 1000m,
            DateMade = DateTime.UtcNow.AddMonths(-1)
        });

        await _client.PostAsJsonAsync($"/assets/{asset.Id}/snapshots", new CreateSnapshotRequest
        {
            TotalValue = 1100m,
            SnapshotDate = DateTime.UtcNow
        });

        // Act
        var response = await _client.GetAsync("/portfolio/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<PortfolioSummaryDto>(_jsonOptions);
        summary.Should().NotBeNull();
        summary!.TotalValue.Should().BeGreaterThan(0);
    }

    #endregion

    #region GET /portfolio/history

    [Fact]
    public async Task GetHistory_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await response.Content.ReadFromJsonAsync<PortfolioHistoryDto>(_jsonOptions);
        history.Should().NotBeNull();
        history!.Points.Should().NotBeNull();
    }

    [Fact]
    public async Task GetHistory_ReturnsPoints_WithDateRange()
    {
        // Act
        var from = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.UtcNow);
        var response = await _client.GetAsync($"/portfolio/history?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region GET /portfolio/allocation

    [Fact]
    public async Task GetAllocation_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/allocation");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var allocation = await response.Content.ReadFromJsonAsync<PortfolioAllocationDto>(_jsonOptions);
        allocation.Should().NotBeNull();
        allocation!.ByType.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllocation_ReturnsPercentages_WithMultipleAssetTypes()
    {
        // Arrange - Create assets of different types
        var etfRequest = new CreateAssetRequest { Name = "Allocation ETF", AssetType = "ETF" };
        var etfResponse = await _client.PostAsJsonAsync("/assets", etfRequest);
        var etf = await etfResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        await _client.PostAsJsonAsync($"/assets/{etf!.Id}/snapshots", new CreateSnapshotRequest
        {
            TotalValue = 5000m,
            SnapshotDate = DateTime.UtcNow
        });

        var cryptoRequest = new CreateAssetRequest { Name = "Allocation Crypto", AssetType = "Crypto" };
        var cryptoResponse = await _client.PostAsJsonAsync("/assets", cryptoRequest);
        var crypto = await cryptoResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        await _client.PostAsJsonAsync($"/assets/{crypto!.Id}/snapshots", new CreateSnapshotRequest
        {
            TotalValue = 3000m,
            SnapshotDate = DateTime.UtcNow
        });

        // Act
        var response = await _client.GetAsync("/portfolio/allocation");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var allocation = await response.Content.ReadFromJsonAsync<PortfolioAllocationDto>(_jsonOptions);
        allocation!.ByType.Should().Contain(a => a.Type == "ETF");
        allocation.ByType.Should().Contain(a => a.Type == "Crypto");
    }

    #endregion

    #region GET /portfolio/returns

    [Fact]
    public async Task GetReturns_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/returns");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returns = await response.Content.ReadFromJsonAsync<PortfolioReturnsDto>(_jsonOptions);
        returns.Should().NotBeNull();
        returns!.Periods.Should().NotBeNull();
    }

    [Fact]
    public async Task GetReturns_AcceptsAsOfDate()
    {
        // Act
        var asOf = DateOnly.FromDateTime(DateTime.UtcNow);
        var response = await _client.GetAsync($"/portfolio/returns?asOf={asOf:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
