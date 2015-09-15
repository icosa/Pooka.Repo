using Pooka.Repo.Utility;

namespace Pooka.Repo.HandlerUtility
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class HandlerReader
    {
        private readonly Func<Type, Type> _keyFromHandlerFn;

        private readonly string _handlerAssemblyName;

        private readonly string _handlersNamespace;

        public HandlerReader(Func<Type, Type> keyFromHandlerFn, string handlerAssemblyName, string handlersNamespace)
        {
            Param.CheckNotNull(keyFromHandlerFn, nameof(keyFromHandlerFn));
            Param.CheckStringNotNullOrEmpty(handlerAssemblyName, nameof(handlerAssemblyName));
            Param.CheckStringNotNullOrEmpty(handlersNamespace, nameof(handlersNamespace));

            _keyFromHandlerFn = keyFromHandlerFn;
            _handlerAssemblyName = handlerAssemblyName;
            _handlersNamespace = handlersNamespace;
        }

        public void ReadHandlers(Action<Type, Type> handlerPairFound)
        {
            Param.CheckNotNull(handlerPairFound, nameof(handlerPairFound));

            var assembly = GetAssemblyByName(_handlerAssemblyName);
            if (null == assembly)
            {
                throw new SystemException("Handler assembly not found in system");
            }

            var handlerTypes = assembly.GetTypes().Where(MatchesHandlerNamespace);
            foreach (var handlerType in handlerTypes)
            {
                var handlerKey = _keyFromHandlerFn(handlerType);
                if (null != handlerKey)
                {
                    handlerPairFound(handlerKey, handlerType);
                }
            }
        }

        private static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(a => name.Equals(a.GetName().Name));
        }

        private bool MatchesHandlerNamespace(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (null == type.Namespace)
            {
                return false;
            }

            return type.Namespace.StartsWith(_handlersNamespace);
        }
    }
}