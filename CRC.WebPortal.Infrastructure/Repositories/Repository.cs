using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using CRC.WebPortal.Domain.Interfaces;
using CRC.WebPortal.Infrastructure.Data;

namespace CRC.WebPortal.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        bool trackChanges = false)
    {
        IQueryable<T> query = _dbSet;

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(
            new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync<TId>(TId id, string includeProperties = "", bool trackChanges = false) where TId : notnull
    {
        var entity = await _dbSet.FindAsync(id);
        
        if (entity == null || string.IsNullOrEmpty(includeProperties))
        {
            return entity;
        }

        var entry = _context.Entry(entity);
        foreach (var includeProperty in includeProperties.Split(
            new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            await entry.Reference(includeProperty.Trim()).LoadAsync();
        }

        return entity;
    }

    public virtual async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string includeProperties = "",
        bool trackChanges = false)
    {
        IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(
            new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        return filter == null 
            ? await _dbSet.CountAsync() 
            : await _dbSet.CountAsync(filter);
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual void DeleteById<TId>(TId id) where TId : notnull
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> condition, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        query = query.Where(condition);

        foreach (var includeProperty in includeProperties.Split(
            new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        query = query.Where(condition);

        foreach (var includeProperty in includeProperties.Split(
            new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdWithIncludesAsync<TId>(TId id, params Expression<Func<T, object>>[] includes) where TId : notnull
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Assuming the entity has an Id property
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id);
        var equal = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

        return await query.FirstOrDefaultAsync(lambda);
    }

    public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }
}
