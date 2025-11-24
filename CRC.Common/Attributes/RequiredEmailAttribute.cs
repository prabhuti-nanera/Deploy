using System.ComponentModel.DataAnnotations;

namespace CRC.Common.Attributes;

/// Custom validation attribute that combines Required and EmailAddress validation
/// Ensures only one error message is returned for email fields

public class RequiredEmailAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // Check if value is null, empty, or whitespace
        if (value is not string email || string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        // Check email format using EmailAddressAttribute logic
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }

    public override string FormatErrorMessage(string name)
    {
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            return "Email is required.";
        }
        return base.FormatErrorMessage(name);
    }
}
