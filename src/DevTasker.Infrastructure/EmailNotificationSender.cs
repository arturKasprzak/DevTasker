using DevTasker.Application;
using DevTasker.Domain;

using Microsoft.Extensions.Logging;

namespace DevTasker.Infrastructure;
public class EmailNotificationSender : INotificationSender
{
    private readonly ILogger<EmailNotificationSender> _logger;

    public EmailNotificationSender(ILogger<EmailNotificationSender> logger)
    {
        _logger = logger;
    }
    public Task SendMessageAsync(string message, User user)
    {
        //TODO change log to sending email
        _logger.LogInformation("Email sent to {UserEmail} with message: {Message}", user.Email, message);

        return Task.CompletedTask;
    }
}