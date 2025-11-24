using CRC.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CRC.Common.Models;

public class VerifyOtpRequest
{
    [RequiredEmail(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required.")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "OTP must be 4 digits.")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "OTP must be 4 digits.")]
    public string OtpCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "New Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string NewPassword { get; set; } = string.Empty;
}
