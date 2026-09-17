using DevTasker.Domain;
using Microsoft.Extensions.Logging;

using Moq;

namespace DevTasker.Application.Tests;

public class TaskAssignedNotificationServiceTests
{
    [Fact]
    public async Task NotifyTaskAssignedAsync_UserIsNotNull_SendMessageAsync()
    {
        //Arrange
        User user = new User("test.user@devtasker.com", "Test User");
        var taskTitle = "Test 1";
        var mockLogger = new Mock<ILogger<TaskAssignedNotificationService>>();
        var mockNotificationSender = new Mock<INotificationSender>();
        var mockUserRepository = new Mock<IUserRepository>();


        mockNotificationSender
            .Setup(sender => sender.SendMessageAsync(It.IsAny<string>(), user))
            .Returns(Task.CompletedTask);
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        var sut = new TaskAssignedNotificationService(mockUserRepository.Object, mockNotificationSender.Object, mockLogger.Object);

        //Act

        await sut.NotifyTaskAssignedAsync(user.Id, taskTitle);

        //Assert

        mockNotificationSender.Verify(sender => sender.SendMessageAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public async Task NotifyTaskAssignedAsync_UserIsNull_UserNotFoundException()
    {
        //Arrange

        var userId = Guid.NewGuid();
        var taskTitle = "Test 1";
        var mockLogger = new Mock<ILogger<TaskAssignedNotificationService>>();
        var mockNotificationSender = new Mock<INotificationSender>();
        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        var sut = new TaskAssignedNotificationService(mockUserRepository.Object, mockNotificationSender.Object, mockLogger.Object);

        //Act
        //Assert

        await Assert.ThrowsAsync<UserNotFoundException>(() => sut.NotifyTaskAssignedAsync(userId, taskTitle));
        mockNotificationSender.Verify(sender => sender.SendMessageAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never());
    }
}
