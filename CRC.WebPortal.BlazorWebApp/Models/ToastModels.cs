namespace CRC.WebPortal.BlazorWebApp.Models;


/// Represents different types of toast notifications
public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}

/// Toast notification model following CQRS pattern
/// Immutable data structure for toast messages
public record ToastMessage
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Message { get; init; } = string.Empty;
    public ToastType Type { get; init; } = ToastType.Info;
    public int DurationMs { get; init; } = 4000;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
  
    /// Factory method for success toast
    public static ToastMessage Success(string message, int durationMs = 4000) => new()
    {
        Message = message,
        Type = ToastType.Success,
        DurationMs = durationMs
    };
    
    /// Factory method for error toast
    public static ToastMessage Error(string message, int durationMs = 5000) => new()
    {
        Message = message,
        Type = ToastType.Error,
        DurationMs = durationMs
    };
    
    /// Factory method for info toast
    public static ToastMessage Info(string message, int durationMs = 4000) => new()
    {
        Message = message,
        Type = ToastType.Info,
        DurationMs = durationMs
    };
  
    /// Factory method for warning toast
    public static ToastMessage Warning(string message, int durationMs = 4000) => new()
    {
        Message = message,
        Type = ToastType.Warning,
        DurationMs = durationMs
    };
}

/// Command for showing toast notification (CQRS Command)
public record ShowToastCommand(ToastMessage Toast);

/// Command for removing toast notification (CQRS Command)
public record RemoveToastCommand(string ToastId);
