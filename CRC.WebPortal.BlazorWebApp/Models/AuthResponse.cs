namespace CRC.WebPortal.BlazorWebApp.Models;

/// Simple response model for authentication operations - UI layer only
public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public UserDto? User { get; set; }
}
