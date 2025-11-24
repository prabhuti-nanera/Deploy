using System;
using System.Threading.Tasks;
using CRC.WebPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace CRC.WebPortal.Infrastructure.Data;

public class DataSeeder : IDisposable
{
    private readonly ILogger<DataSeeder> _logger;
    private bool _disposed = false;

    public DataSeeder(ILogger<DataSeeder> logger)
    {
        _logger = logger;
    }

    public async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            // Check if data already exists
            if (await context.Users.AnyAsync())
            {
                _logger.LogInformation("Database already has data. Skipping seeding.");
                return;
            }

            _logger.LogInformation("Seeding database...");

            // Create a test user
            var testUser = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsActive = true,
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Hash the password using BCrypt
            testUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123");

            await context.Users.AddAsync(testUser);
            await context.SaveChangesAsync();

            _logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    // Password hashing is now handled by BCrypt

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources here
            }
            _disposed = true;
        }
    }

    ~DataSeeder()
    {
        Dispose(false);
    }
}
