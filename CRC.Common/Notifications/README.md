# CRC Toast Notification System

A reusable, production-ready toast notification system for Blazor applications following Clean Architecture principles.

## Features

- ✅ **Clean Architecture** - Proper separation of concerns
- ✅ **Reusable** - Can be used across multiple projects
- ✅ **Type-safe** - Full TypeScript-like experience with C#
- ✅ **Customizable** - Easy to style and extend
- ✅ **Accessible** - Follows accessibility best practices
- ✅ **Animated** - Smooth slide-in/out animations
- ✅ **Auto-dismiss** - Configurable auto-dismiss timing
- ✅ **Manual close** - Users can manually close notifications

## Quick Start

### 1. Register Services

In your `Program.cs`:

```csharp
using CRC.Common.Notifications.Extensions;

// Add CRC Notifications
builder.Services.AddCRCNotifications();
```

### 2. Add Component to Layout

In your main layout (e.g., `App.razor`):

```razor
@using CRC.Common.Notifications.Components

<Router AppAssembly="@typeof(App).Assembly">
    <!-- Your existing router content -->
</Router>

<CRCToastNotification />
```

### 3. Use in Components

**Option A: Inherit from CRCBaseComponent**

```razor
@using CRC.Common.Notifications.Components
@inherits CRCBaseComponent

<button @onclick="ShowSuccess">Show Success</button>
<button @onclick="ShowError">Show Error</button>

@code {
    private async Task ShowSuccess()
    {
        await ShowSuccessToastAsync("Operation completed successfully!", 3000);
    }

    private async Task ShowError()
    {
        await ShowErrorToastAsync("An error occurred while processing your request.", 4000);
    }
}
```

**Option B: Inject Service Directly**

```razor
@using CRC.Common.Notifications.Interfaces
@inject INotificationService NotificationService

<button @onclick="ShowNotification">Show Notification</button>

@code {
    private async Task ShowNotification()
    {
        await NotificationService.ShowSuccessAsync("Hello from CRC Notifications!");
    }
}
```

## API Reference

### NotificationType Enum

```csharp
public enum NotificationType
{
    Success,  // Green - for successful operations
    Error,    // Red - for errors and failures
    Info,     // Blue - for informational messages
    Warning   // Yellow - for warnings
}
```

### INotificationService Methods

```csharp
Task ShowSuccessAsync(string message, int durationMs = 4000);
Task ShowErrorAsync(string message, int durationMs = 4000);
Task ShowInfoAsync(string message, int durationMs = 4000);
Task ShowWarningAsync(string message, int durationMs = 4000);
Task ShowNotificationAsync(string message, NotificationType type, int durationMs = 4000);
```

### CRCBaseComponent Methods

```csharp
protected Task ShowSuccessToastAsync(string message, int durationMs = 4000);
protected Task ShowErrorToastAsync(string message, int durationMs = 4000);
protected Task ShowInfoToastAsync(string message, int durationMs = 4000);
protected Task ShowWarningToastAsync(string message, int durationMs = 4000);
```

## Styling

The component uses CSS classes prefixed with `crc-toast-` to avoid conflicts:

- `.crc-toast-container` - Main container
- `.crc-toast` - Individual toast
- `.crc-toast-success` - Success toast styling
- `.crc-toast-error` - Error toast styling
- `.crc-toast-warning` - Warning toast styling
- `.crc-toast-info` - Info toast styling

## Examples

### Form Validation

```csharp
private async Task HandleValidSubmit()
{
    try
    {
        var result = await MyService.SaveAsync(model);
        if (result.IsSuccess)
        {
            await ShowSuccessToastAsync("Data saved successfully!", 3000);
        }
        else
        {
            await ShowErrorToastAsync(result.Message ?? "Failed to save data.", 4000);
        }
    }
    catch (Exception ex)
    {
        await ShowErrorToastAsync("An unexpected error occurred.", 4000);
    }
}
```

### API Error Handling

```csharp
private async Task HandleInvalidSubmit()
{
    await ShowErrorToastAsync("Please correct the errors in the form.", 4000);
}
```

## Integration with Existing Projects

This notification system is designed to be a drop-in replacement for any existing toast notification system. Simply:

1. Install the CRC.Common package reference
2. Register services with `AddCRCNotifications()`
3. Add `<CRCToastNotification />` to your layout
4. Replace existing notification calls with CRC notification methods

## Architecture

```
CRC.Common.Notifications/
├── Interfaces/
│   └── INotificationService.cs      # Service contract
├── Models/
│   └── NotificationModels.cs        # Data models
├── Components/
│   ├── CRCToastNotification.razor   # UI component
│   └── CRCBaseComponent.cs          # Base class for easy usage
├── Extensions/
│   └── ServiceCollectionExtensions.cs # DI registration
└── NotificationService.cs           # Service implementation
```

This follows Clean Architecture principles with clear separation of concerns and dependency inversion.
