using Microsoft.AspNetCore.Components;
using CRC.Common.Notifications.Interfaces;

namespace CRC.Common.Notifications.Components;


/// Base component that provides easy access to notification methods
/// Any Blazor component can inherit from this to get notification capabilities

public abstract class CRCBaseComponent : ComponentBase
{
    [Inject] protected INotificationService NotificationService { get; set; } = default!;

    /// Shows a success notification
    protected async Task ShowSuccessToastAsync(string message, int durationMs = 4000)
    {
        await NotificationService.ShowSuccessAsync(message, durationMs);
    }

    /// Shows an error notification
    protected async Task ShowErrorToastAsync(string message, int durationMs = 4000)
    {
        await NotificationService.ShowErrorAsync(message, durationMs);
    }

    /// Shows an info notification
    protected async Task ShowInfoToastAsync(string message, int durationMs = 4000)
    {
        await NotificationService.ShowInfoAsync(message, durationMs);
    }

    /// Shows a warning notification
    protected async Task ShowWarningToastAsync(string message, int durationMs = 4000)
    {
        await NotificationService.ShowWarningAsync(message, durationMs);
    }

    /// Shows a notification with specified type
    protected async Task ShowNotificationAsync(string message, CRC.Common.Notifications.Models.NotificationType type, int durationMs = 4000)
    {
        await NotificationService.ShowNotificationAsync(message, type, durationMs);
    }
}
