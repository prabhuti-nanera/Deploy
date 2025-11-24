namespace CRC.WebPortal.Application.Features.Auth.Queries.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }

    public class OtpResponse
    {
        public string Email { get; set; } = string.Empty;
        public bool IsSent { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
