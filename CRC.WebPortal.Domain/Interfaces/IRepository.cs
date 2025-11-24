using System.Linq.Expressions;

namespace CRC.WebPortal.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    // Get all entities
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        bool trackChanges = false);

    // Get entity by ID
    Task<T?> GetByIdAsync<TId>(TId id, string includeProperties = "", bool trackChanges = false) where TId : notnull;
    
    // Get first or default entity based on filter
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string includeProperties = "",
        bool trackChanges = false);

    // Check if any entity exists with the given filter
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    
    // Count entities with optional filter
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    
    // Add operations
    void Add(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
    Task AddAsync(T entity);
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    // Update operations
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    
    // Delete operations
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    void DeleteById<TId>(TId id) where TId : notnull;
    
    // Save changes
    Task<int> SaveChangesAsync();
    
    // Additional methods for complex queries
    Task<T?> GetByConditionAsync(Expression<Func<T, bool>> condition, string includeProperties = "");
    Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition, string includeProperties = "");
    Task<T?> GetByIdWithIncludesAsync<TId>(TId id, params Expression<Func<T, object>>[] includes) where TId : notnull;
    Task<IEnumerable<T>> GetAllWithIncludesAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
}
