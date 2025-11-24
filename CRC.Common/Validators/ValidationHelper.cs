using System.ComponentModel.DataAnnotations;

namespace CRC.Common.Validators;

/// Validation helper for password strength validation
public static class ValidationHelper
{
    /// Validates password strength based on requirements
    public static bool IsStrongPassword(string password, int minLength = 8)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < minLength)
            return false;

        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}
