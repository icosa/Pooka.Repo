namespace Pooka.Repo.EF
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Contracts.Queries;

    public class QuerySet<TEntity> : IQuerySet<TEntity> where TEntity : class
    {
        private readonly IQueryable<TEntity> _dbQuery;

        public QuerySet(IQueryable<TEntity> dbQuery)
        {
            _dbQuery = dbQuery;
        }

        public Expression Expression => _dbQuery.Expression;

        public Type ElementType => _dbQuery.ElementType;

        public IQueryProvider Provider => _dbQuery.Provider;

        public IQuerySet<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return new QuerySet<TEntity>(_dbQuery.Include(path));
        }

        public IQuerySet<TEntity> AsNoTracking()
        {
            return new QuerySet<TEntity>(_dbQuery.AsNoTracking());
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            var enumerable = _dbQuery as IEnumerable<TEntity>;
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dbQuery.GetEnumerator();
        }

#if old_code_can_be_deleted
        public Expression Expression
        {
            get { return AsIDbSet.Expression; }
        }

        public Type ElementType
        {
            get { return AsIDbSet.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return AsIDbSet.Provider; }
        }

        private IDbSet<TEntity> AsIDbSet
        {
            get { return _dbSet; }
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            var enumerable = _dbSet as IEnumerable<TEntity>;
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return _dbSet.FindAsync(keyValues);
        }
#endif
    }
}