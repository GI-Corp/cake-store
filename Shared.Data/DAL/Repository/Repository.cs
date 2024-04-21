using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Abstraction;
using Shared.Domain.Entities.Abstraction;

namespace Shared.Data.DAL.Repository;

public class Repository<TEntity, Tkey, TDbContext> : IRepository<TEntity, Tkey, TDbContext>, IDisposable
    where TEntity : class, IEntity<Tkey> where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly DbSet<TEntity> _set;


    public Repository(TDbContext dbContext)
    {
        _dbContext = dbContext;
        _set = _dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null)
    {
        var query = _set.Where(predicate);

        if (skip.HasValue) 
            query = query.Skip(skip.Value);

        if (take.HasValue) 
            query = query.Take(take.Value);
        
        return query.ToListAsync();
    }

    public Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _set.Where(predicate);
        _set.RemoveRange(query);

        return SaveChangesAsync();
    }

    public void Add(TEntity entity)
    {
        _set.Add(entity);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.AnyAsync(predicate);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.CountAsync(predicate);
    }

    public Task<bool> CreateAsync(TEntity entity)
    {
        _set.Add(entity);
        
        return SaveChangesAsync();
    }

    public Task<bool> DeleteAsync(Tkey id)
    {
        var value = Expression.Constant(id);
        var item = Expression.Parameter(typeof(TEntity), "entity");
        var prop = Expression.Property(item, "Id");
        var equal = Expression.Equal(prop, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);

        var query = _set.Where(lambda);

        _set.RemoveRange(query);

        return SaveChangesAsync();
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        _set.Remove(entity);

        return SaveChangesAsync();
    }

    public Task<TEntity> FindByIdAsync(Tkey id)
    {
        var value = Expression.Constant(id);
        var item = Expression.Parameter(typeof(TEntity), "entity");
        var prop = Expression.Property(item, "Id");
        var equal = Expression.Equal(prop, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
        return SingleOrDefaultAsync(lambda);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.SingleOrDefaultAsync(predicate);
    }

    public Task<List<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetIncluding(includeProperties);
        query = query.Where(predicate);

        return query.ToListAsync();
    }

    protected IQueryable<TEntity> GetIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _set;
        return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.FirstOrDefaultAsync(predicate);
    }

    public Task<bool> CreateManyAsync(IEnumerable<TEntity> entities)
    {
        _set.AddRange(entities);

        return SaveChangesAsync();
    }

    public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> property)
    {
        return _set.MaxAsync(property);
    }


    // Synchronous calls

    #region Synchronous calls

    public List<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null)
    {
        var query = _set.Where(predicate);
        
        if (skip.HasValue) 
            query = query.Skip(skip.Value);

        if (take.HasValue) 
            query = query.Take(take.Value);

        return query.ToList();
    }

    public bool DeleteMany(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _set.Where(predicate);
        _set.RemoveRange(query);

        return SaveChanges();
    }


    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.Any(predicate);
    }

    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.Count(predicate);
    }

    public bool Create(TEntity entity)
    {
        _set.Add(entity);

        return SaveChanges();
    }

    public bool Delete(Tkey id)
    {
        var value = Expression.Constant(id);
        var item = Expression.Parameter(typeof(TEntity), "entity");
        var prop = Expression.Property(item, "Id");
        var equal = Expression.Equal(prop, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);

        var query = _set.Where(lambda);

        _set.RemoveRange(query);

        return SaveChanges();
    }

    public bool Delete(TEntity entity)
    {
        _set.Remove(entity);

        return SaveChanges();
    }

    public TEntity FindById(Tkey id)
    {
        var value = Expression.Constant(id);
        var item = Expression.Parameter(typeof(TEntity), "entity");
        var prop = Expression.Property(item, "Id");
        var equal = Expression.Equal(prop, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);

        return SingleOrDefault(lambda);
    }

    public bool SaveChanges()
    {
        return _dbContext.SaveChanges() > 0;
    }

    public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.SingleOrDefault(predicate);
    }

    public List<TEntity> GetIncluding(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = GetIncluding(includeProperties);
        query = query.Where(predicate);

        return query.ToList();
    }

    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.FirstOrDefault(predicate);
    }

    public bool CreateMany(IEnumerable<TEntity> entities)
    {
        _set.AddRange(entities);

        return SaveChanges();
    }

    public TResult Max<TResult>(Expression<Func<TEntity, TResult>> property)
    {
        return _set.Max(property);
    }

    #endregion

    #region IDisposable implementation

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) _dbContext.Dispose();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}