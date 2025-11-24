using System.ComponentModel.DataAnnotations;
using CRC.Common.Validators;

namespace CRC.Common.Attributes;

/// Custom validation attribute for strong password validation with length check
/// Based on BMS validation patterns
public class StrongPasswordAttribute : ValidationAttribute
{
    public int MinLength { get; set; } = ValidationRules.PASSWORD_MIN_LENGTH;
    public int MaxLength { get; set; } = ValidationRules.PASSWORD_MAX_LENGTH;

    public override bool IsValid(object? value)
    {
        if (value is not string password) return false;
        
        // Check length first
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinLength || password.Length > MaxLength)
            return false;
            
        return ValidationHelper.IsStrongPassword(password, MinLength);
    }

    public override string FormatErrorMessage(string name)
    {
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            return "Password is invalid.";
        }
        return base.FormatErrorMessage(name);
    }
}
