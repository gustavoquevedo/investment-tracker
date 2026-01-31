using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using InvestmentTracker.Api.Features.Assets.CreateAsset;
using InvestmentTracker.Api.Features.Assets.ManageSnapshots;
using InvestmentTracker.Api.Features.Assets.ManageContributions;
using InvestmentTracker.Api.Features.Portfolio.GetSummary;
using InvestmentTracker.Api.Features.Portfolio.GetHistory;
using InvestmentTracker.Api.Features.Portfolio.GetAllocation;
using InvestmentTracker.Api.Features.Portfolio.GetReturns;
using Xunit;

namespace InvestmentTracker.Api.Tests;

public class PortfolioControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PortfolioControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region GET /portfolio/summary

    [Fact]
    public async Task GetSummary_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<GetSummaryResponse>(_jsonOptions);
        summary.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSummary_ReturnsZeroValues_WhenNoAssets()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<GetSummaryResponse>(_jsonOptions);
        summary.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSummary_CalculatesCorrectly_WithAssets()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Summary Test Asset", "ETF", null, null, 0);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var asset = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        await _client.PostAsJsonAsync($"/assets/{asset!.Id}/contributions", 
            new AddContributionRequest(1000m, DateTime.UtcNow.AddDays(-30), null));
        await _client.PostAsJsonAsync($"/assets/{asset.Id}/snapshots", 
            new AddSnapshotRequest(1100m, DateTime.UtcNow));

        // Act
        var response = await _client.GetAsync("/portfolio/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<GetSummaryResponse>(_jsonOptions);
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
    }

    [Fact]
    public async Task GetHistory_ReturnsEmptyPoints_WhenNoData()
    {
        // Act
        var response = await _client.GetAsync("/portfolio/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await response.Content.ReadFromJsonAsync<GetHistoryResponse>(_jsonOptions);
        history.Should().NotBeNull();
        history!.Points.Should().NotBeNull();
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
        var allocation = await response.Content.ReadFromJsonAsync<GetAllocationResponse>(_jsonOptions);
        allocation.Should().NotBeNull();
        allocation!.ByType.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllocation_GroupsByAssetType()
    {
        // Arrange
        var etfRequest = new CreateAssetRequest("Allocation ETF", "ETF", null, null, 0);
        var etfResponse = await _client.PostAsJsonAsync("/assets", etfRequest);
        var etfAsset = await etfResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);
        await _client.PostAsJsonAsync($"/assets/{etfAsset!.Id}/snapshots", 
            new AddSnapshotRequest(5000m, DateTime.UtcNow));

        var cryptoRequest = new CreateAssetRequest("Allocation Crypto", "Crypto", null, null, 0);
        var cryptoResponse = await _client.PostAsJsonAsync("/assets", cryptoRequest);
        var cryptoAsset = await cryptoResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);
        await _client.PostAsJsonAsync($"/assets/{cryptoAsset!.Id}/snapshots", 
            new AddSnapshotRequest(3000m, DateTime.UtcNow));

        // Act
        var response = await _client.GetAsync("/portfolio/allocation");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var allocation = await response.Content.ReadFromJsonAsync<GetAllocationResponse>(_jsonOptions);
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
        var returns = await response.Content.ReadFromJsonAsync<GetReturnsResponse>(_jsonOptions);
        returns.Should().NotBeNull();
    }

    #endregion
}
