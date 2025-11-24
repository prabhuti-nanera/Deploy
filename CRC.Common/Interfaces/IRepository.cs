using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using CRC.Common.Models;

namespace CRC.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull;
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<BaseResponse<T>> AddAsync(T entity);
        Task<BaseResponse> UpdateAsync(T entity);
        Task<BaseResponse> DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
