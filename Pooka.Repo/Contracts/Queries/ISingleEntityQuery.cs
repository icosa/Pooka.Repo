namespace Pooka.Repo.Contracts.Queries
{
    using System.Threading.Tasks;

    public interface ISingleEntityQuery<TResult> where TResult : class
    {
        Task<TResult> FindAsync(IQueryContext context);
    }
}