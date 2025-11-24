using CRC.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRC.WebPortal.Application.Common.Models;

public class AuthResponse : BaseResponse<AuthResponse>
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public UserDto? User { get; set; }
    public bool IsSuccess => Succeeded;
    
    public static new AuthResponse Success(string? message = null) => new() 
    { 
        Succeeded = true, 
        Message = message ?? string.Empty,
        Data = null
    };
    
    public static AuthResponse Failure(string message, IEnumerable<string>? errors = null) => new() 
    { 
        Succeeded = false, 
        Message = message,
        Errors = errors?.ToArray(),
        Data = null
    };
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsEmailVerified { get; set; }
}
