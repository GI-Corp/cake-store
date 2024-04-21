using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities.Abstraction;

namespace Shared.Data.Abstraction;

public interface IRepository<TEntity, TKey, TDbContext> where TEntity : IEntity<TKey> where TDbContext : DbContext
{
    Task<TEntity> FindByIdAsync(TKey id);
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null);
    Task<List<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> CreateAsync(TEntity entity);
    Task<bool> CreateManyAsync(IEnumerable<TEntity> entities);
    Task<bool> SaveChangesAsync();
    void Add(TEntity entity);

    Task<bool> DeleteAsync(TKey id);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
    
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> predicate);

    #region Synchronous calls

    TEntity FindById(TKey id);
    TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    List<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null);
    List<TEntity> GetIncluding(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    bool Any(Expression<Func<TEntity, bool>> predicate);
    bool Create(TEntity entity);
    bool CreateMany(IEnumerable<TEntity> entities);
    bool SaveChanges();

    
    bool Delete(TKey id);
    bool Delete(TEntity entity);
    bool DeleteMany(Expression<Func<TEntity, bool>> predicate);
    
    int Count(Expression<Func<TEntity, bool>> predicate);
    TResult Max<TResult>(Expression<Func<TEntity, TResult>> predicate);

    #endregion
}