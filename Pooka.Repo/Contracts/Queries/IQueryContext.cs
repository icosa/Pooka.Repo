namespace Pooka.Repo.Contracts.Queries
{
    public interface IQueryContext
    {
        IQuerySet<T> SetOf<T>() where T : class;

        string ConnectionString { get; }

        void EnableTrace();
    }
}