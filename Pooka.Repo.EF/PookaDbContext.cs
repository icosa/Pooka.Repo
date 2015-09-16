namespace Pooka.Repo.EF
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Queries;
    using Utility;

    internal class PookaDbContext : IRepository
    {
        private readonly DbContext _dbContext;

        private readonly string _connectionString;

        public PookaDbContext(DbContext dbContext, string connectionString)
        {
            Param.CheckNotNull(dbContext, nameof(dbContext));
            Param.CheckStringNotNullOrEmpty(connectionString, nameof(connectionString));

            _dbContext = dbContext;
            _connectionString = connectionString;
        }

        public Task<T> FindByIdAsync<T>(int id) where T : class
        {
#if NET4
            T result = _dbContext.Set<T>().Find(id);
            return TaskEx.FromResult(result);
#else
            return _dbContext.Set<T>().FindAsync(id);
#endif
        }

        public Task<T> FindAsync<T>(ISingleEntityQuery<T> query) where T : class
        {
            return query.FindAsync(new QueryContext(_dbContext, _connectionString));
        }

        public Task<T[]> FindAsync<T>(IEntityQuery<T> query) where T : class
        {
            return query.FindAsync(new QueryContext(_dbContext, _connectionString));
        }

        public Task<T[]> GetAllAsync<T>() where T : class
        {
#if NET4
            var results = _dbContext.Set<T>().ToArray();
            return TaskEx.FromResult(results);
#else
            return _dbContext.Set<T>().ToArrayAsync();
#endif
        }

        public async Task<T> AddAsync<T>(T item, bool commit = false) where T : class
        {
            _dbContext.Set<T>().Add(item);
            if (commit)
            {
                await SaveChangesAsync();
            }

            return item;
        }

        public async Task RemoveAsync<T>(T item, bool commit = false) where T : class
        {
            var dbSet = _dbContext.Set<T>();
            var dbEntityEntry = _dbContext.Entry(item);
            if ((null == dbEntityEntry) || (EntityState.Detached == dbEntityEntry.State))
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);
            if (commit)
            {
                await SaveChangesAsync();
            }
        }

        public void AttachExisting<T>(T entity) where T : class
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Attach(entity);
        }

        public Task<int> SaveChangesAsync()
        {
#if NET4
            int result = _dbContext.SaveChanges();
            return TaskEx.FromResult(result);
#else
            return _dbContext.SaveChangesAsync();
#endif
        }
    }
}
