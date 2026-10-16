using DevTasker.Domain;

using Moq;

namespace DevTasker.Application.Tests;
public class TaskServiceTests
{
    [Fact]
    public async Task GetAllTaskItemsAsync_WhenCalled_ReturnsTasksFromRepository()
    {
        // Arrange

        var mockTaskRepository = new Mock<ITaskRepository>();
        var expectedTask = new List<TaskItem>
        {
            new ("Title", null, null, null)
        };

        mockTaskRepository
            .Setup(repo => repo.GetAllTaskItemsAsync())
            .ReturnsAsync(expectedTask);

        var sut = new TaskService(mockTaskRepository.Object);

        // Act

        var result = await sut.GetAllTaskItemsAsync();

        // Assert

        Assert.Equal(expectedTask, result);
        mockTaskRepository.Verify(repo => repo.GetAllTaskItemsAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewTaskItemAsync_WhenCalled_InvokesRepository()
    {
        // Arrange

        var mockTaskRepository = new Mock<ITaskRepository>();
        var newTaskItem = new TaskItem("Title", null, null, null);

        mockTaskRepository
            .Setup(repo => repo.AddNewTaskItemAsync(newTaskItem))
            .Returns(Task.CompletedTask);

        var sut = new TaskService(mockTaskRepository.Object);

        // Act

        await sut.AddNewTaskItemAsync(newTaskItem);

        // Assert

        mockTaskRepository.Verify(repo => repo.AddNewTaskItemAsync(newTaskItem), Times.Once);
    }

    [Fact]
    public async Task GetTaskItemAsync_WhenCalled_ReturnsTaskFromRepository()
    {
        // Arrange

        var mockTaskRepository = new Mock<ITaskRepository>();

        var taskItem = new TaskItem("Title", null, null, null);

        mockTaskRepository
            .Setup(repo => repo.GetTaskItemAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var sut = new TaskService(mockTaskRepository.Object);

        // Act

        var result = await sut.GetTaskItemAsync(taskItem.Id);

        // Assert

        Assert.Equal(taskItem, result);
        mockTaskRepository.Verify(repo => repo.GetTaskItemAsync(taskItem.Id), Times.Once);
    }

    [Fact]
    public async Task RemoveTaskItemAsync_WhenCalled_ReturnsRemovedTask()
    {

        // Arrange

        var mockTaskRepository = new Mock<ITaskRepository>();
        var taskItem = new TaskItem("Title", null, null, null);

        mockTaskRepository
            .Setup(repo => repo.RemoveTaskItemAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var sut = new TaskService(mockTaskRepository.Object);

        // Act

        var result = await sut.RemoveTaskItemAsync(taskItem.Id);

        // Assert

        Assert.Equal(taskItem, result);
        mockTaskRepository.Verify(repo => repo.RemoveTaskItemAsync(taskItem.Id), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskItemAsync_WhenCalled_ReturnsTaskFromRepository()
    {

        // Arrange

        var mockTaskRepository = new Mock<ITaskRepository>();
        var oldTitle = "Title";
        var newTitle = "New Title";
        var taskItem = new TaskItem(oldTitle, null, null, null);
        var request = new UpdateTaskItemRequest(newTitle, null, null, null);

        mockTaskRepository
            .Setup(repo => repo.UpdateTaskItemAsync(taskItem.Id, request))
            .ReturnsAsync(taskItem);

        var sut = new TaskService(mockTaskRepository.Object);

        // Act

        var result = await sut.UpdateTaskItemAsync(taskItem.Id, request);

        // Assert

        Assert.Equal(taskItem, result);
        mockTaskRepository.Verify(repo => repo.UpdateTaskItemAsync(taskItem.Id, request), Times.Once);
    }
}
