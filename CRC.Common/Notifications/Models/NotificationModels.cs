namespace CRC.Common.Notifications.Models;

/// Represents the type of notification to display
public enum NotificationType
{
    Success,
    Error,
    Info,
    Warning
}

/// Represents a notification message with all its properties
public class NotificationMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public int DurationMs { get; set; } = 4000;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVisible { get; set; } = false;
    public int AnimationDelay { get; set; } = 0;
}

/// Event arguments for notification events
public class NotificationEventArgs : EventArgs
{
    public NotificationMessage Notification { get; set; } = new();

    public NotificationEventArgs(NotificationMessage notification)
    {
        Notification = notification;
    }
}

/// Response model for notification operations
public class NotificationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string NotificationId { get; set; } = string.Empty;
}
