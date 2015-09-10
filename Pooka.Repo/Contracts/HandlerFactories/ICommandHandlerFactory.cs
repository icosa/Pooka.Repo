namespace Pooka.Repo.Contracts.HandlerFactories
{
    using Commands;

    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> GetHandler<T>(IRepository repository);         
    }
}