using System.ComponentModel.DataAnnotations;

namespace CRC.Common.Attributes;

/// Custom validation attribute to ensure no leading/trailing whitespace
public class NoWhitespaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string stringValue)
            return true; // Let other validators handle null/non-string values

        return stringValue == stringValue.Trim();
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} cannot have leading or trailing whitespace.";
    }
}
