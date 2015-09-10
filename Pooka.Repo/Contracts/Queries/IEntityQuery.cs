namespace Pooka.Repo.Contracts.Queries
{
    using System.Threading.Tasks;

    public interface IEntityQuery<TResult> where TResult : class
    {
        Task<TResult[]> FindAsync(IQueryContext context);
    }
}