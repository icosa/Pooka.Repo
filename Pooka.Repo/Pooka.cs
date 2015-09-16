namespace Pooka.Repo
{
    using System;
    using Contracts.Commands;
    using Contracts.Queries;
    using HandlerUtility;
    using System.Linq;
    using Contracts;

    public class Pooka
    {
        static public IPookaHandlers BuildHandlerCollectionFromAssembly(string assemblyName, string commandNamespace, string queryNamespace)
        {
            var commandHandlers = new HandlerCollectionBuilder(assemblyName, commandNamespace, GetKeyFromCommandHandlerType);
            var queryHandlers = new HandlerCollectionBuilder(assemblyName, queryNamespace, GetKeyFromQueryHandlerType);
            return new PookaHandlers(commandHandlers, queryHandlers);
        }

        static public IDomain CreateDomain(string connectionString, Func<IRepository> repositoryFactory, IPookaHandlers handlers)
        {
            var dbCommandFactory = new PookaCommandFactory(handlers.CommandHandlers);
            var dbQueryFactory = new PookaQueryFactory(handlers.QueryHandlers);
            return new Domain(connectionString, repositoryFactory, dbCommandFactory, dbQueryFactory);
        }

        private static bool MatchsQueryTypeInterface(Type interfaceType)
        {
            if (interfaceType.IsGenericType)
            {
                var genericType = interfaceType.GetGenericTypeDefinition();
                if ((genericType == typeof(ISingleEntityQuery<>)) || (genericType == typeof(IEntityQuery<>)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchsCommandTypeInterface(Type interfaceType)
        {
            if (interfaceType.IsGenericType)
            {
                var genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(ICommandHandler<>))
                {
                    return true;
                }
            }

            return false;
        }

        private static Type GetKeyFromCommandHandlerType(Type commandHandlerType)
        {
            var commandInterface = commandHandlerType.GetInterfaces().FirstOrDefault(MatchsCommandTypeInterface);

            var genericTypeArguments = commandInterface?.GetGenericArguments();
            return genericTypeArguments?[0];
        }

        private static Type GetKeyFromQueryHandlerType(Type queryHandlerType)
        {
            if (!queryHandlerType.GetInterfaces().Any(MatchsQueryTypeInterface))
            {
                return null;
            }

            var constructorInfos = queryHandlerType.GetConstructors();
            if (constructorInfos.Length != 1)
            {
                return null;
            }

            var parameterInfos = constructorInfos[0].GetParameters();
            if (parameterInfos.Length != 1)
            {
                return null;
            }

            return parameterInfos[0].ParameterType;
        }
    }
}