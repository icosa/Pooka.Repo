namespace Pooka.Repo
{
    using System;
    using System.Linq;

    using Contracts.Commands;
    using Contracts.Queries;
    using HandlerUtility;

    public class DbQueryCommandHandlers
    {
        public DbQueryCommandHandlers(string handlerAssemblyName, string commandNamespace, string queryNamespace)
        {
            CommandHandles = new HandlerCollection(handlerAssemblyName, commandNamespace, GetKeyFromCommandHandlerType);
            QueryHandlers = new HandlerCollection(handlerAssemblyName, queryNamespace, GetKeyFromQueryHandlerType);
        }

        public HandlerCollection CommandHandles { get; }

        public HandlerCollection QueryHandlers { get; }

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