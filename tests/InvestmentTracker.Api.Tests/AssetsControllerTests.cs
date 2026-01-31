using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using InvestmentTracker.Api.Features.Assets.GetAssets;
using InvestmentTracker.Api.Features.Assets.GetAssetById;
using InvestmentTracker.Api.Features.Assets.CreateAsset;
using InvestmentTracker.Api.Features.Assets.UpdateAsset;
using InvestmentTracker.Api.Features.Assets.ManageSnapshots;
using InvestmentTracker.Api.Features.Assets.ManageContributions;
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
        var assets = await response.Content.ReadFromJsonAsync<List<GetAssetsResponse>>(_jsonOptions);
        assets.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ReturnsList_WhenAssetsExist()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Test Asset", "ETF", null, null, 0.5m);
        await _client.PostAsJsonAsync("/assets", createRequest);

        // Act
        var response = await _client.GetAsync("/assets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var assets = await response.Content.ReadFromJsonAsync<List<GetAssetsResponse>>(_jsonOptions);
        assets.Should().NotBeNull();
    }

    #endregion

    #region GET /assets/{id}

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/assets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ReturnsAsset_WhenExists()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Test Asset By Id", "ETF", null, null, 0.5m);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        // Act
        var response = await _client.GetAsync($"/assets/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var asset = await response.Content.ReadFromJsonAsync<GetAssetByIdResponse>(_jsonOptions);
        asset.Should().NotBeNull();
        asset!.Name.Should().Be("Test Asset By Id");
    }

    #endregion

    #region POST /assets

    [Fact]
    public async Task Create_ReturnsCreated_WithValidAsset()
    {
        // Arrange
        var request = new CreateAssetRequest("New ETF", "ETF", "IE00B4L5Y983", "VWCE", 0.22m);

        // Act
        var response = await _client.PostAsJsonAsync("/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var asset = await response.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);
        asset.Should().NotBeNull();
        asset!.Name.Should().Be("New ETF");
        asset.AssetType.Should().Be("ETF");
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidAssetType()
    {
        // Arrange
        var request = new CreateAssetRequest("Invalid Asset", "InvalidType", null, null, 0);

        // Act
        var response = await _client.PostAsJsonAsync("/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PUT /assets/{id}

    [Fact]
    public async Task Update_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Arrange
        var request = new UpdateAssetRequest("Updated Name", null, null, null, null);

        // Act
        var response = await _client.PutAsJsonAsync("/assets/99999", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenAssetExists()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Asset To Update", "ETF", null, null, 0.5m);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        var updateRequest = new UpdateAssetRequest("Updated Name", null, null, null, null);

        // Act
        var response = await _client.PutAsJsonAsync($"/assets/{created!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<UpdateAssetResponse>(_jsonOptions);
        updated!.Name.Should().Be("Updated Name");
    }

    #endregion

    #region DELETE /assets/{id}

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/assets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenAssetExists()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Asset To Delete", "ETF", null, null, 0.5m);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/assets/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    #endregion

    #region GET /assets/{id}/snapshots

    [Fact]
    public async Task GetSnapshots_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/assets/99999/snapshots");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSnapshots_ReturnsEmptyList_WhenNoSnapshots()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Asset Without Snapshots", "ETF", null, null, 0.5m);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        // Act
        var response = await _client.GetAsync($"/assets/{created!.Id}/snapshots");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var snapshots = await response.Content.ReadFromJsonAsync<List<GetSnapshotsResponse>>(_jsonOptions);
        snapshots.Should().NotBeNull();
        snapshots.Should().BeEmpty();
    }

    #endregion

    #region POST /assets/{id}/snapshots

    [Fact]
    public async Task AddSnapshot_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Arrange
        var request = new AddSnapshotRequest(1000m, DateTime.UtcNow);

        // Act
        var response = await _client.PostAsJsonAsync("/assets/99999/snapshots", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddSnapshot_ReturnsCreated_WhenAssetExists()
    {
        // Arrange
        var createRequest = new CreateAssetRequest("Asset With Snapshot", "ETF", null, null, 0.5m);
        var createResponse = await _client.PostAsJsonAsync("/assets", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateAssetResponse>(_jsonOptions);

        var snapshotRequest = new AddSnapshotRequest(1000m, DateTime.UtcNow);

        // Act
        var response = await _client.PostAsJsonAsync($"/assets/{created!.Id}/snapshots", snapshotRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region GET /assets/{id}/contributions

    [Fact]
    public async Task GetContributions_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/assets/99999/contributions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
