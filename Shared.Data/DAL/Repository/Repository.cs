using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Abstraction;
using Shared.Domain.Entities.Abstraction;

namespace Shared.Data.DAL.Repository
{
    public class Repository<TEntity, TKey, TDbContext> : IRepository<TEntity, TKey, TDbContext>
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _set;

        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _set = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity?> FindByIdAsync(TKey id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            return await _set.SingleOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity?>> GetAsync(Expression<Func<TEntity?, bool>> predicate, int? skip = null, int? take = null)
        {
            var query = _set.Where(predicate);
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);
            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetIncluding(includeProperties);
            query = query.Where(predicate);
            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            return await _set.AnyAsync(predicate);
        }

        public async Task<bool> CreateAsync(TEntity? entity)
        {
            _set.Add(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                return await SaveChangesAsync();
            }
            return false;
        }

        public async Task<bool> DeleteAsync(TEntity? entity)
        {
            _set.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteManyAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            var entities = await _set.Where(predicate).ToListAsync();
            if (entities.Any())
            {
                _set.RemoveRange(entities);
                return await SaveChangesAsync();
            }
            return false;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            return await _set.CountAsync(predicate);
        }

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> predicate)
        {
            return await _set.MaxAsync(predicate);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Add(TEntity? entity)
        {
            _set.Add(entity);
        }

        protected IQueryable<TEntity> GetIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity?> query = _set;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
