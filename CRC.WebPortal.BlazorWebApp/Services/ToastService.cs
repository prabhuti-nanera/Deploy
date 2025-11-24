using CRC.WebPortal.BlazorWebApp.Models;
using System.Collections.Concurrent;

namespace CRC.WebPortal.BlazorWebApp.Services;

/// Toast notification service implementation following CQRS pattern
/// Thread-safe implementation using ConcurrentDictionary
/// Handles automatic toast removal with timers
public class ToastService : IToastService, IDisposable
{
    private readonly ConcurrentDictionary<string, ToastMessage> _activeToasts = new();
    private readonly ConcurrentDictionary<string, Timer> _toastTimers = new();
    private bool _disposed = false;

 
    /// Event triggered when a new toast is added
    public event EventHandler<ToastMessage>? OnToastAdded;
    

    /// Event triggered when a toast is removed

    public event EventHandler<string>? OnToastRemoved;

    /// Gets all active toast messages as read-only list

    public IReadOnlyList<ToastMessage> ActiveToasts => 
        _activeToasts.Values.OrderBy(t => t.CreatedAt).ToList();

    /// Shows a success toast message
    public async Task ShowSuccessAsync(string message, int durationMs = 4000)
    {
        var toast = ToastMessage.Success(message, durationMs);
        await ShowToastAsync(toast);
    }


    /// Shows an error toast message
    public async Task ShowErrorAsync(string message, int durationMs = 5000)
    {
        var toast = ToastMessage.Error(message, durationMs);
        await ShowToastAsync(toast);
    }


    /// Shows an info toast message
    public async Task ShowInfoAsync(string message, int durationMs = 4000)
    {
        var toast = ToastMessage.Info(message, durationMs);
        await ShowToastAsync(toast);
    }


    /// Shows a warning toast message
    public async Task ShowWarningAsync(string message, int durationMs = 4000)
    {
        var toast = ToastMessage.Warning(message, durationMs);
        await ShowToastAsync(toast);
    }

    /// Shows a custom toast message
    /// Implements the Command pattern for toast operations
    public async Task ShowToastAsync(ToastMessage toast)
    {
        if (_disposed) return;

        // Add toast to active collection
        _activeToasts.TryAdd(toast.Id, toast);

        // Create auto-removal timer
        var timer = new Timer(async _ => await RemoveToastAsync(toast.Id), 
                             null, 
                             TimeSpan.FromMilliseconds(toast.DurationMs), 
                             Timeout.InfiniteTimeSpan);
        
        _toastTimers.TryAdd(toast.Id, timer);

        // Notify subscribers (UI components)
        OnToastAdded?.Invoke(this, toast);
        
        await Task.CompletedTask;
    }

    /// Removes a specific toast by ID
    public async Task RemoveToastAsync(string toastId)
    {
        if (_disposed) return;

        // Remove toast from active collection
        _activeToasts.TryRemove(toastId, out _);

        // Dispose and remove timer
        if (_toastTimers.TryRemove(toastId, out var timer))
        {
            timer.Dispose();
        }

        // Notify subscribers
        OnToastRemoved?.Invoke(this, toastId);
        
        await Task.CompletedTask;
    }

    /// Clears all active toasts
    public async Task ClearAllToastsAsync()
    {
        if (_disposed) return;

        var toastIds = _activeToasts.Keys.ToList();
        
        foreach (var toastId in toastIds)
        {
            await RemoveToastAsync(toastId);
        }
    }

    /// Dispose pattern implementation
    /// Ensures proper cleanup of timers and resources
    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        // Dispose all timers
        foreach (var timer in _toastTimers.Values)
        {
            timer.Dispose();
        }

        _toastTimers.Clear();
        _activeToasts.Clear();

        GC.SuppressFinalize(this);
    }
}
