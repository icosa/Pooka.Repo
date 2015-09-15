namespace Pooka.Repo.HandlerUtility
{
    using System;
    using Contracts;
    using Contracts.Commands;
    using Contracts.HandlerFactories;

    internal class PookaCommandFactory : ICommandHandlerFactory
    {
        private readonly HandlerCollectionBuilder _handlerCollectionBuilder;

        public PookaCommandFactory(HandlerCollectionBuilder handlerCollectionBuilder)
        {
            if (handlerCollectionBuilder == null) throw new ArgumentNullException(nameof(handlerCollectionBuilder));

            _handlerCollectionBuilder = handlerCollectionBuilder;
        }

        public ICommandHandler<T> GetHandler<T>(IRepository repository)
        {
            var commandHandlerType = _handlerCollectionBuilder.TryGetHandlerType(typeof(T));
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