using System.ComponentModel.DataAnnotations;
using CRC.Common.Attributes;
using CRC.Common.Validators;

namespace CRC.Common.Models;

/// Common signup request model with enhanced validation attributes
/// Can be used across Web, API, MAUI, and other .NET applications
public class SignupRequest
{
    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(ValidationRules.FIRSTNAME_MAX_LENGTH, MinimumLength = ValidationRules.FIRSTNAME_MIN_LENGTH)]
    [RegularExpression(@"^[a-zA-Z\s]+$")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(ValidationRules.LASTNAME_MAX_LENGTH, MinimumLength = ValidationRules.LASTNAME_MIN_LENGTH)]
    [RegularExpression(@"^[a-zA-Z\s]+$")]
    public string LastName { get; set; } = string.Empty;

    [RequiredEmail(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StrongPassword]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
