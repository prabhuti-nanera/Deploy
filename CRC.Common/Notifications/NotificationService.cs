using CRC.Common.Notifications.Interfaces;
using CRC.Common.Notifications.Models;

namespace CRC.Common.Notifications;

/// Default implementation of the notification service
public class NotificationService : INotificationService
{
    public event EventHandler<NotificationEventArgs>? OnNotificationAdded;

    public async Task ShowSuccessAsync(string message, int durationMs = 4000)
    {
        await ShowNotificationAsync(message, NotificationType.Success, durationMs);
    }

    public async Task ShowErrorAsync(string message, int durationMs = 4000)
    {
        await ShowNotificationAsync(message, NotificationType.Error, durationMs);
    }

    public async Task ShowInfoAsync(string message, int durationMs = 4000)
    {
        await ShowNotificationAsync(message, NotificationType.Info, durationMs);
    }

    public async Task ShowWarningAsync(string message, int durationMs = 4000)
    {
        await ShowNotificationAsync(message, NotificationType.Warning, durationMs);
    }

    public async Task ShowNotificationAsync(string message, NotificationType type, int durationMs = 4000)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        // Ensure duration is within reasonable bounds
        var duration = Math.Max(1000, Math.Min(10000, durationMs));

        var notification = new NotificationMessage
        {
            Message = message,
            Type = type,
            DurationMs = duration,
            CreatedAt = DateTime.UtcNow
        };

        // Trigger the notification event
        OnNotificationAdded?.Invoke(this, new NotificationEventArgs(notification));

        // Return completed task for async compliance
        await Task.CompletedTask;
    }
}
