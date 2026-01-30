using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using InvestmentTracker.Api.DTOs;
using Xunit;

namespace InvestmentTracker.Api.Tests;

public class AssetsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AssetsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region GET /assets

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoAssets()
    {
        // Act
        var response = await _client.GetAsync("/assets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var assets = await response.Content.ReadFromJsonAsync<List<AssetResponse>>(_jsonOptions);
        assets.Should().NotBeNull();
    }

    #endregion

    #region POST /assets

    [Fact]
    public async Task Create_ReturnsCreated_WithValidAsset()
    {
        // Arrange
        var request = new CreateAssetRequest
        {
            Name = "Test ETF",
            AssetType = "ETF",
            ISIN = "IE00B4L5Y983",
            Ticker = "IWDA"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var asset = await response.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);
        asset.Should().NotBeNull();
        asset!.Name.Should().Be("Test ETF");
        asset.AssetType.Should().Be("ETF");
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidAssetType()
    {
        // Arrange
        var request = new { Name = "Test", AssetType = "InvalidType" };

        // Act
        var response = await _client.PostAsJsonAsync("/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region GET /assets/{id}

    [Fact]
    public async Task GetById_ReturnsAsset_WhenExists()
    {
        // Arrange - Create an asset first
        var createRequest = new CreateAssetRequest { Name = "GetById Test", AssetType = "Stock" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        // Act
        var response = await _client.GetAsync($"/assets/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var asset = await response.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);
        asset!.Id.Should().Be(created.Id);
        asset.Name.Should().Be("GetById Test");
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/assets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region PUT /assets/{id}

    [Fact]
    public async Task Update_ReturnsOk_WithValidUpdate()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "Original Name", AssetType = "Cash" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        var updateRequest = new UpdateAssetRequest { Name = "Updated Name" };

        // Act
        var response = await _client.PutAsJsonAsync($"/assets/{created!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);
        updated!.Name.Should().Be("Updated Name");
    }

    #endregion

    #region DELETE /assets/{id}

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "To Delete", AssetType = "Crypto" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/assets/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify it's gone
        var getResponse = await _client.GetAsync($"/assets/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region POST /assets/{id}/snapshots

    [Fact]
    public async Task AddSnapshot_ReturnsCreated_WithValidSnapshot()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "Snapshot Test", AssetType = "Fund" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var asset = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        var snapshotRequest = new CreateSnapshotRequest
        {
            TotalValue = 10000.50m,
            SnapshotDate = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/assets/{asset!.Id}/snapshots", snapshotRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region GET /assets/{id}/snapshots

    [Fact]
    public async Task GetSnapshots_ReturnsSnapshots_AfterAdding()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "Snapshots List Test", AssetType = "ETF" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var asset = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        var snapshotRequest = new CreateSnapshotRequest
        {
            TotalValue = 5000m,
            SnapshotDate = DateTime.UtcNow
        };
        await _client.PostAsJsonAsync($"/assets/{asset!.Id}/snapshots", snapshotRequest);

        // Act
        var response = await _client.GetAsync($"/assets/{asset.Id}/snapshots");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region POST /assets/{id}/contributions

    [Fact]
    public async Task AddContribution_ReturnsCreated_WithValidContribution()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "Contribution Test", AssetType = "Pension" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var asset = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        var contributionRequest = new CreateContributionRequest
        {
            Amount = 500.00m,
            DateMade = DateTime.UtcNow,
            Note = "Monthly contribution"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/assets/{asset!.Id}/contributions", contributionRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region GET /assets/{id}/contributions

    [Fact]
    public async Task GetContributions_ReturnsContributions_AfterAdding()
    {
        // Arrange
        var createRequest = new CreateAssetRequest { Name = "Contributions List Test", AssetType = "Stock" };
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var asset = await createResponse.Content.ReadFromJsonAsync<AssetResponse>(_jsonOptions);

        var contributionRequest = new CreateContributionRequest
        {
            Amount = 1000m,
            DateMade = DateTime.UtcNow
        };
        await _client.PostAsJsonAsync($"/assets/{asset!.Id}/contributions", contributionRequest);

        // Act
        var response = await _client.GetAsync($"/assets/{asset.Id}/contributions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
