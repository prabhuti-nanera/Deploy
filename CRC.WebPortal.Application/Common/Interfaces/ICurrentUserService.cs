using System.Security.Claims;

namespace CRC.WebPortal.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
    ClaimsPrincipal? Principal { get; }
    
    bool IsInRole(string role);
    bool HasClaim(string claimType, string claimValue);
    string? GetClaimValue(string claimType);
}
