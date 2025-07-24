
using AcademiaHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AcademiaHub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AcademiaHubDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AcademiaHubDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);
        public void Update(T entity) => _dbSet.Update(entity);
        public void UpdateRange(List<T> entities) => _dbSet.UpdateRange(entities);

        public void Delete(T entity) =>  _dbSet.Remove(entity);
        public void DeleteRange(List<T> entities) => _dbSet.RemoveRange(entities);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
                                      IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>,
                                      IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        
    }
}
