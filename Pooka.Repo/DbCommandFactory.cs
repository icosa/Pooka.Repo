namespace Pooka.Repo
{
    using System;

    using EnsureThat;

    using Contracts;
    using Contracts.Commands;
    using Contracts.HandlerFactories;
    using HandlerUtility;

    public class DbCommandFactory : ICommandHandlerFactory
    {
        private readonly HandlerCollection _handlerCollection;

        public DbCommandFactory(HandlerCollection handlerCollection)
        {
            Ensure.That(handlerCollection).IsNotNull();

            _handlerCollection = handlerCollection;
        }

        public ICommandHandler<T> GetHandler<T>(IRepository repository)
        {
            var commandHandlerType = _handlerCollection.TryGetHandlerType(typeof(T));
            if (null == commandHandlerType)
            {
                throw new InvalidOperationException("Command handler not registered!");
            }

            if (commandHandlerType.IsGenericType)
            {
                var commandType = typeof(T);
                var genericArg = commandType;
                if (commandType.IsGenericType)
                {
                    genericArg = commandType.GetGenericArguments()[0];
                }

                commandHandlerType = commandHandlerType.MakeGenericType(genericArg);                    
            }

            var handlerInstance = Activator.CreateInstance(commandHandlerType, repository);
            return (ICommandHandler<T>)handlerInstance;
        }
    }
}