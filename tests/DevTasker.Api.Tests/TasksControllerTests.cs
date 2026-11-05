using DevTasker.Api.Controllers;
using DevTasker.Application;
using DevTasker.Domain;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace DevTasker.Api.Tests;
public class TasksControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly TasksController _sut;

    public TasksControllerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _mockUserRepo = new Mock<IUserRepository>();
        _sut = new TasksController(_mockTaskService.Object, _mockUserRepo.Object);
    }

    [Fact]
    public async Task GetById_TaskNotExist_ReturnsNotFound()
    {
        // Arrange

        var newId = Guid.NewGuid();

        _mockTaskService
            .Setup(service => service.GetTaskItemAsync(newId))
            .ReturnsAsync((TaskItem?)null);

        // Act
        // Assert

        await Assert.ThrowsAsync<TaskNotFoundException>(
            () => _sut.GetById(newId));
    }

    [Fact]
    public async Task GetById_TaskExist_ReturnsTask()
    {
        // Arrange 

        var newTaskItem = new TaskItem("Title", null, null, null);

        _mockTaskService
            .Setup(service => service.GetTaskItemAsync(newTaskItem.Id))
            .ReturnsAsync(newTaskItem);

        // Act

        var result = await _sut.GetById(newTaskItem.Id);

        // Assert

        Assert.IsType<OkObjectResult>(result);
        _mockTaskService.Verify(s => s.GetTaskItemAsync(newTaskItem.Id), Times.Once);
    }
}
