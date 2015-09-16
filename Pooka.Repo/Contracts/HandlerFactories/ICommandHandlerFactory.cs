namespace Pooka.Repo.Contracts.HandlerFactories
{
    using System;
    using Commands;

    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> GetHandler<T>(Func<IRepository> repositoryFactory, string connectionString);
    }
}