using CRC.WebPortal.BlazorWebApp.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

/// Toast notification service interface following CQRS pattern
/// Provides clean separation between UI and notification logic

public interface IToastService
{
 
    /// Event triggered when a new toast is added
    /// Follows Observer pattern for reactive UI updates
    event EventHandler<ToastMessage>? OnToastAdded;
    

    /// Event triggered when a toast is removed
    event EventHandler<string>? OnToastRemoved;
    
 
    /// Gets all active toast messages
    IReadOnlyList<ToastMessage> ActiveToasts { get; }
    
    /// Shows a success toast message
    Task ShowSuccessAsync(string message, int durationMs = 4000);

    /// Shows an error toast message
    Task ShowErrorAsync(string message, int durationMs = 5000);
    
 
    /// Shows an info toast message
    Task ShowInfoAsync(string message, int durationMs = 4000);
    

    /// Shows a warning toast message
    Task ShowWarningAsync(string message, int durationMs = 4000);
    
    /// Shows a custom toast message
    Task ShowToastAsync(ToastMessage toast);
    
    /// Removes a specific toast by ID
    Task RemoveToastAsync(string toastId);
    
    /// Clears all active toasts

    Task ClearAllToastsAsync();
}
