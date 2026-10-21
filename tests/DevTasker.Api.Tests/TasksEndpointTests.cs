using System.Net;
using System.Net.Http.Json;

using DevTasker.Api.Tests.Factories;

namespace DevTasker.Api.Tests;

public class TasksEndpointTests : IClassFixture<DevTaskerWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TasksEndpointTests(DevTaskerWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_WhenCalled_ReturnsOk()
    {
        // Act

        var response = await _client.GetAsync("/api/tasks");

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_WhenCalled_ReturnsCreated()
    {
        // Act

        var response = await _client.PostAsJsonAsync("/api/tasks", new
        {
            title = "Test Task",
            description = (string?)null,
            status = (bool?)null,
            userId = (Guid?)null
        });

        // Assert

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetTaskById_NonExistentId_ReturnsNotFound()
    {
        // Arrange

        var nonExistentId = Guid.NewGuid();

        // Act

        var response = await _client.GetAsync($"/api/tasks/{nonExistentId}");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WhenCalled_ReturnsNoContent()
    {
        // Arrange                

        var createResponse = await _client.PostAsJsonAsync("/api/tasks", new
        {
            title = "Test Task",
            description = (string?)null,
            status = (bool?)null,
            userId = (Guid?)null
        });

        var createdTaskId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Act

        var response = await _client.DeleteAsync($"/api/tasks/{createdTaskId}");

        // Assert

        Assert.NotNull(createdTaskId);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
