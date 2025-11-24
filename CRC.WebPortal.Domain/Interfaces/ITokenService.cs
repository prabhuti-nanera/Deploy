using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Domain.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
}
