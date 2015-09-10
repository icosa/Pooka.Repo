namespace Pooka.Repo
{
    using System;
    using System.Threading.Tasks;

    using Contracts;
    using Contracts.HandlerFactories;
    using Contracts.Queries;

    public class Domain : IDomain
    {
        private readonly IRepository _repository;

        private readonly ICommandHandlerFactory _commandHandlerFactory;

        private readonly IQueryHandlerFactory _queryHandlerFactory;

        private bool _repositoryDisposed;

        public Domain(
            IRepository dataContext,
            ICommandHandlerFactory commandHandlerFactory,
            IQueryHandlerFactory queryHandlerFactory)
        {
            _repository = dataContext;
            _commandHandlerFactory = commandHandlerFactory;
            _queryHandlerFactory = queryHandlerFactory;
        }

        public async Task<T> FindByIdAsync<T>(int id) where T : class
        {
            return await _repository.FindByIdAsync<T>(id);            
        }

        public async Task<T> FindSingleAsync<T>(QueryParameters<T> queryParameters) where T : class
        {
            ISingleEntityQuery<T> queryHandler = _queryHandlerFactory.GetHandlerSingle(queryParameters);
            if (queryHandler == null)
            {
                throw new SystemException("No command handler found for " + queryParameters.GetType());
            }

            return await _repository.FindAsync(queryHandler);
        }

        public async Task<T[]> FindAsync<T>(QueryParameters<T> queryParameters) where T : class
        {
            IEntityQuery<T> queryHandler = _queryHandlerFactory.GetHandler(queryParameters);
            if (queryHandler == null)
            {
                throw new SystemException("No command handler found for " + queryParameters.GetType());
            }

            return await _repository.FindAsync(queryHandler);
        }

        public async Task<T[]> GetAllAsync<T>() where T : class
        {
            return await _repository.GetAllAsync<T>();
        }

        public Task ExecuteAsync<T>(T command)
        {
            var handler = _commandHandlerFactory.GetHandler<T>(_repository);
            if (handler == null)
            {
                throw new SystemException("No command handler found for " + typeof(T));
            }

            return handler.ExecuteAsync(command);
        }

        public void Dispose()
        {
            if (!_repositoryDisposed)
            {
                var disposable = _repository as IDisposable;
                disposable?.Dispose();

                _repositoryDisposed = true;
            }
        }
    }
}