namespace Pooka.Repo.HandlerUtility
{
    using System;
    using Contracts.HandlerFactories;
    using Contracts.Queries;

    public class PookaQueryFactory : IQueryHandlerFactory
    {
        private readonly HandlerCollectionBuilder _handlerCollectionBuilder;

        public PookaQueryFactory(HandlerCollectionBuilder handlerCollectionBuilder)
        {
            if (handlerCollectionBuilder == null) throw new ArgumentNullException(nameof(handlerCollectionBuilder));

            _handlerCollectionBuilder = handlerCollectionBuilder;
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
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            var queryHandlerType = _handlerCollectionBuilder.TryGetHandlerType(queryParameters.GetType());
            if (null == queryHandlerType)
            {
                throw new InvalidOperationException("Query handler not registered!");
            }

            return queryHandlerType;
        }
    }
}