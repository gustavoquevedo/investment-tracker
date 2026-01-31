using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using InvestmentTracker.Api.Features.Tags.GetTags;
using InvestmentTracker.Api.Features.Tags.CreateTag;
using Xunit;

namespace InvestmentTracker.Api.Tests;

public class TagsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public TagsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region GET /tags

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region POST /tags

    [Fact]
    public async Task Create_ReturnsCreated_WithValidTag()
    {
        // Arrange
        var request = new CreateTagRequest("Test Tag", "#FF5733");

        // Act
        var response = await _client.PostAsJsonAsync("/tags", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var tag = await response.Content.ReadFromJsonAsync<CreateTagResponse>(_jsonOptions);
        tag.Should().NotBeNull();
        tag!.Name.Should().Be("Test Tag");
        tag.ColorHex.Should().Be("#FF5733");
    }

    #endregion

    #region DELETE /tags/{id}

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        // Arrange - Create a tag first
        var createRequest = new CreateTagRequest("To Delete Tag", "#000000");
        var createResponse = await _client.PostAsJsonAsync("/tags", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTagResponse>(_jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/tags/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/tags/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
