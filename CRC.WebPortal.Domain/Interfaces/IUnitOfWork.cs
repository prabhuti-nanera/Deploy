using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositories
    IUserRepository Users { get; }
    
    // Methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Transaction support
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    // Alias for SaveChangesAsync for backward compatibility
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
