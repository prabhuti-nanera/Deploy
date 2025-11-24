using Microsoft.EntityFrameworkCore;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.WebPortal.Infrastructure.Data;

namespace CRC.WebPortal.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
    }
}
