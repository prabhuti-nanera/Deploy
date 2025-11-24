using CRC.Common.Notifications.Models;

namespace CRC.Common.Notifications.Interfaces;


/// Service interface for managing notifications across the application
public interface INotificationService
{
    /// Event triggered when a new notification is added

    event EventHandler<NotificationEventArgs>? OnNotificationAdded;

    /// Shows a success notification
    Task ShowSuccessAsync(string message, int durationMs = 4000);

    /// Shows an error notification
    Task ShowErrorAsync(string message, int durationMs = 4000);


    /// Shows an info notification
    Task ShowInfoAsync(string message, int durationMs = 4000);


    /// Shows a warning notification
    Task ShowWarningAsync(string message, int durationMs = 4000);

    /// Shows a notification with specified type
    Task ShowNotificationAsync(string message, NotificationType type, int durationMs = 4000);
}
