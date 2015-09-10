namespace Pooka.Repo
{
    using System.Data.Entity;

    using Contracts.Queries;

    public class QueryContext : IQueryContext
    {
        private readonly DbContext _context;

        public QueryContext(DbContext context)
        {
            _context = context;
        }
 
        public IQuerySet<T> SetOf<T>() where T : class
        {
            return new QuerySet<T>(_context.Set<T>());
        }

        public void EnableTrace()
        {
#if DEBUG
            _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }
    }
}