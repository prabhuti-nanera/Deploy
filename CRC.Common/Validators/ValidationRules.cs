namespace CRC.Common.Validators;

/// Centralized validation rules and constants based on BMS patterns
public static class ValidationRules
{
    // Authentication Rules (Based on BMS analysis)
    public const int USERNAME_MAX_LENGTH = 100;
    public const int PASSWORD_MAX_LENGTH = 255;
    public const int PASSWORD_MIN_LENGTH = 8;
    public const int EMAIL_MAX_LENGTH = 100;
    public const int FIRSTNAME_MAX_LENGTH = 50;
    public const int FIRSTNAME_MIN_LENGTH = 2;
    public const int LASTNAME_MAX_LENGTH = 50;
    public const int LASTNAME_MIN_LENGTH = 2;
    public const int OTP_LENGTH = 6;
    
    /// Error codes for consistent error handling
    public static class ErrorCodes
    {
        // Authentication Error Codes
        public const string AUTH_EMAIL_REQUIRED = "AUTH_001";
        public const string AUTH_EMAIL_INVALID = "AUTH_002";
        public const string AUTH_EMAIL_TOO_LONG = "AUTH_003";
        public const string AUTH_PASSWORD_REQUIRED = "AUTH_004";
        public const string AUTH_PASSWORD_TOO_SHORT = "AUTH_005";
        public const string AUTH_PASSWORD_TOO_LONG = "AUTH_006";
        public const string AUTH_PASSWORD_WEAK = "AUTH_007";
        public const string AUTH_PASSWORDS_NOT_MATCH = "AUTH_008";
        
        // User Information Error Codes
        public const string USER_FIRSTNAME_REQUIRED = "USER_001";
        public const string USER_FIRSTNAME_TOO_SHORT = "USER_002";
        public const string USER_FIRSTNAME_TOO_LONG = "USER_003";
        public const string USER_FIRSTNAME_INVALID_FORMAT = "USER_004";
        public const string USER_FIRSTNAME_WHITESPACE = "USER_005";
        
        public const string USER_LASTNAME_REQUIRED = "USER_006";
        public const string USER_LASTNAME_TOO_SHORT = "USER_007";
        public const string USER_LASTNAME_TOO_LONG = "USER_008";
        public const string USER_LASTNAME_INVALID_FORMAT = "USER_009";
        public const string USER_LASTNAME_WHITESPACE = "USER_010";
        
        // OTP Error Codes
        public const string OTP_REQUIRED = "OTP_001";
        public const string OTP_INVALID_LENGTH = "OTP_002";
        public const string OTP_INVALID_FORMAT = "OTP_003";
        
        // Business Logic Error Codes
        public const string BUSINESS_EMAIL_EXISTS = "BIZ_001";
        public const string BUSINESS_ACCOUNT_INACTIVE = "BIZ_002";
        public const string BUSINESS_ACCOUNT_LOCKED = "BIZ_003";
        public const string BUSINESS_INVALID_CREDENTIALS = "BIZ_004";
    }
    
    /// Error messages for user-friendly display
    public static class ErrorMessages
    {
        // Authentication Messages
        public const string EMAIL_REQUIRED = "Email is required";
        public const string EMAIL_INVALID = "Invalid email format";
        public const string EMAIL_TOO_LONG = "Email cannot exceed {0} characters";
        public const string PASSWORD_REQUIRED = "Password is required";
        public const string PASSWORD_TOO_SHORT = "Password must be at least {0} characters";
        public const string PASSWORD_TOO_LONG = "Password cannot exceed {0} characters";
        public const string PASSWORD_WEAK = "Password must contain at least 8 characters with uppercase, lowercase, number, and special character";
        public const string PASSWORDS_NOT_MATCH = "Passwords do not match";
        
        // User Information Messages
        public const string FIRSTNAME_REQUIRED = "First Name is required";
        public const string FIRSTNAME_TOO_SHORT = "First Name must be at least {0} characters";
        public const string FIRSTNAME_TOO_LONG = "First Name cannot exceed {0} characters";
        public const string FIRSTNAME_INVALID_FORMAT = "First Name can only contain letters and spaces";
        public const string FIRSTNAME_WHITESPACE = "First Name cannot have leading or trailing spaces";
        
        public const string LASTNAME_REQUIRED = "Last Name is required";
        public const string LASTNAME_TOO_SHORT = "Last Name must be at least {0} characters";
        public const string LASTNAME_TOO_LONG = "Last Name cannot exceed {0} characters";
        public const string LASTNAME_INVALID_FORMAT = "Last Name can only contain letters and spaces";
        public const string LASTNAME_WHITESPACE = "Last Name cannot have leading or trailing spaces";
        
        // OTP Messages
        public const string OTP_REQUIRED = "OTP code is required";
        public const string OTP_INVALID_LENGTH = "OTP code must be exactly {0} digits";
        public const string OTP_INVALID_FORMAT = "OTP code must contain only digits";
        
        // Business Logic Messages
        public const string EMAIL_EXISTS = "An account with this email already exists";
        public const string ACCOUNT_INACTIVE = "Your account has been deactivated. Please contact support";
        public const string ACCOUNT_LOCKED = "Account is locked due to multiple failed login attempts";
        public const string INVALID_CREDENTIALS = "Invalid email or password. Please check your credentials and try again";
    }
}
