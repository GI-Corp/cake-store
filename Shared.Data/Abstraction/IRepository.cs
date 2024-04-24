using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities.Abstraction;

namespace Shared.Data.Abstraction
{
    public interface IRepository<TEntity, TKey, TDbContext> : IDisposable
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext
    {
        Task<TEntity?> FindByIdAsync(TKey id);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity?, bool>> predicate);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity?, bool>> predicate);
        Task<List<TEntity?>> GetAsync(Expression<Func<TEntity?, bool>> predicate, int? skip = null, int? take = null);
        Task<List<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> predicate);
        Task<bool> CreateAsync(TEntity? entity);
        Task<bool> SaveChangesAsync();
        void Add(TEntity? entity);

        Task<bool> DeleteAsync(TKey id);
        Task<bool> DeleteAsync(TEntity? entity);
        Task<bool> DeleteManyAsync(Expression<Func<TEntity?, bool>> predicate);
        
        Task<int> CountAsync(Expression<Func<TEntity?, bool>> predicate);
        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> predicate);
    }
}