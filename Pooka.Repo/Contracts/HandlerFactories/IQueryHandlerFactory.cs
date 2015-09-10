namespace Pooka.Repo.Contracts.HandlerFactories
{
    using Queries;

    public interface IQueryHandlerFactory
    {
        IEntityQuery<T> GetHandler<T>(QueryParameters<T> queryParameters) where T : class;

        ISingleEntityQuery<T> GetHandlerSingle<T>(QueryParameters<T> queryParameters) where T : class;
    }
}