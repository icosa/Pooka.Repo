namespace Pooka.Repo
{
    using System;

    using EnsureThat;

    using Contracts.HandlerFactories;
    using Contracts.Queries;
    using HandlerUtility;

    public class DbQueryFactory : IQueryHandlerFactory
    {
        private readonly HandlerCollection _handlerCollection;

        public DbQueryFactory(HandlerCollection handlerCollection)
        {
            Ensure.That(handlerCollection).IsNotNull();

            _handlerCollection = handlerCollection;
        }

        public IEntityQuery<T> GetHandler<T>(QueryParameters<T> queryParameters) where T : class
        {
            var queryHandlerType = GetHandlerFromCollection(queryParameters);
            return (IEntityQuery<T>)Activator.CreateInstance(queryHandlerType, queryParameters);
        }

        public ISingleEntityQuery<T> GetHandlerSingle<T>(QueryParameters<T> queryParameters) where T : class
        {
            var queryHandlerType = GetHandlerFromCollection(queryParameters);
            return (ISingleEntityQuery<T>)Activator.CreateInstance(queryHandlerType, queryParameters);
        }

        private Type GetHandlerFromCollection<T>(QueryParameters<T> queryParameters) where T : class
        {
            Ensure.That(queryParameters).IsNotNull();

            var queryHandlerType = _handlerCollection.TryGetHandlerType(queryParameters.GetType());
            if (null == queryHandlerType)
            {
                throw new InvalidOperationException("Query handler not registered!");
            }

            return queryHandlerType;
        }
    }
}