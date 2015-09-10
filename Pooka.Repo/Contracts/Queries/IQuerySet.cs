using System;
using System.Linq.Expressions;

namespace Pooka.Repo.Contracts.Queries
{
    using System.Linq;

    public interface IQuerySet<TEntity> : IQueryable<TEntity>
    {
        IQuerySet<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path);

        IQuerySet<TEntity> AsNoTracking();

#if old_code_can_be_deleted
        Task<TEntity> FindAsync(params object[] keyValues);
#endif
    }
}