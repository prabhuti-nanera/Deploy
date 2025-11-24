using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByEmailAsync(string email);
}
