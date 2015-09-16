namespace Pooka.Repo.HandlerUtility
{
    using System;
    using System.Reflection;
    using Contracts;
    using Contracts.Commands;
    using Contracts.HandlerFactories;

    internal class PookaCommandFactory : ICommandHandlerFactory
    {
        private readonly HandlerCollectionBuilder _handlerCollectionBuilder;

        private static readonly object[] EmptyParameterList = { };

        public PookaCommandFactory(HandlerCollectionBuilder handlerCollectionBuilder)
        {
            if (handlerCollectionBuilder == null) throw new ArgumentNullException(nameof(handlerCollectionBuilder));

            _handlerCollectionBuilder = handlerCollectionBuilder;
        }

        public ICommandHandler<T> GetHandler<T>(Func<IRepository> repositoryFactory, string connectionString)
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

            object[] args = GetCtorArguments(commandHandlerType, repositoryFactory, connectionString);
            var handlerInstance = Activator.CreateInstance(commandHandlerType, args);
            return (ICommandHandler<T>)handlerInstance;
        }

        private object[] GetCtorArguments(Type commandHandlerType, Func<IRepository> repositoryFactory, string connectionString)
        {
            ConstructorInfo[] ctors = commandHandlerType.GetConstructors();
            if (0 == ctors.Length)
            {
                return EmptyParameterList;
            }

            // assuming class A has only one constructor
            var ctor = ctors[0];
            ParameterInfo[] parameterInfo = ctor.GetParameters();
            if (1 != parameterInfo.Length)
            {
                return EmptyParameterList;
            }

            var parameterType = parameterInfo[0].ParameterType;
            if ((parameterType == typeof(string)) || (parameterType == typeof(String)))
            {
                return new object[] { connectionString };
            }

            if (parameterType != typeof(IRepository))
            {
                throw new InvalidOperationException("Unrecognized command handler constructor!");
            }

            return new object[] { repositoryFactory() };
        }
    }
}