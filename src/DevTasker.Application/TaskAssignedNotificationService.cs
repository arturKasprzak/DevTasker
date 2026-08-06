using Microsoft.Extensions.Logging;

namespace DevTasker.Application;

public class TaskAssignedNotificationService
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationSender _notificationSender;
    private readonly ILogger<TaskAssignedNotificationService> _logger;

    public TaskAssignedNotificationService(IUserRepository userRepository, INotificationSender notificationSender, ILogger<TaskAssignedNotificationService> logger)
    {
        _userRepository = userRepository;
        _notificationSender = notificationSender;
        _logger = logger;
    }

    public async Task NotifyTaskAssignedAsync(Guid userId, string taskTitle)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("User {UserId} was not found", userId);
            throw new UserNotFoundException(userId);
        }

        var message = $"You have been assigned a task: {taskTitle}";

        await _notificationSender.SendMessageAsync(message, user);

        _logger.LogInformation("Notification about task {TaskTitle} sent to user {UserId}", taskTitle, userId);
    }
}
