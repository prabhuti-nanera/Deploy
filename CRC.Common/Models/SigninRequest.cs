using System.ComponentModel.DataAnnotations;
using CRC.Common.Validators;
using CRC.Common.Attributes;

namespace CRC.Common.Models;

/// Common signin request model with enhanced validation attributes
/// Can be used across Web, API, MAUI, and other .NET applications
public class SigninRequest
{
    [RequiredEmail(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
