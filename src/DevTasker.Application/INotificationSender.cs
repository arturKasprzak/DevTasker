using DevTasker.Domain;

namespace DevTasker.Application;

public interface INotificationSender
{
    Task SendMessageAsync(string message, User user);
}