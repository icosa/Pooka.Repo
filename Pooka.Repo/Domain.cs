namespace Pooka.Repo
{
    using System;
    using System.Threading.Tasks;

    using Contracts;
    using Contracts.HandlerFactories;
    using Contracts.Queries;

    internal class Domain : IDomain
    {
        private readonly Lazy<IRepository> _repository;

        private readonly string _connectionString;

        private readonly ICommandHandlerFactory _commandHandlerFactory;

        private readonly IQueryHandlerFactory _queryHandlerFactory;

        private bool _repositoryDisposed;

        public Domain(
            string connectionString,
            Func<IRepository> repositoryFactory,
            ICommandHandlerFactory commandHandlerFactory,
            IQueryHandlerFactory queryHandlerFactory)
        {
            _repository = new Lazy<IRepository>(repositoryFactory);
            _connectionString = connectionString;
            _commandHandlerFactory = commandHandlerFactory;
            _queryHandlerFactory = queryHandlerFactory;
        }

        public async Task<T> FindByIdAsync<T>(int id) where T : class
        {
            return await GetRepository().FindByIdAsync<T>(id);            
        }

        public async Task<T> FindSingleAsync<T>(QueryParameters<T> queryParameters) where T : class
        {
            ISingleEntityQuery<T> queryHandler = _queryHandlerFactory.GetHandlerSingle(queryParameters);
            if (queryHandler == null)
            {
                throw new SystemException("No command handler found for " + queryParameters.GetType());
            }

            return await GetRepository().FindAsync(queryHandler);
        }

        public async Task<T[]> FindAsync<T>(QueryParameters<T> queryParameters) where T : class
        {
            IEntityQuery<T> queryHandler = _queryHandlerFactory.GetHandler(queryParameters);
            if (queryHandler == null)
            {
                throw new SystemException("No command handler found for " + queryParameters.GetType());
            }

            return await GetRepository().FindAsync(queryHandler);
        }

        public async Task<T[]> GetAllAsync<T>() where T : class
        {
            return await GetRepository().GetAllAsync<T>();
        }

        public Task ExecuteAsync<T>(T command)
        {
            var handler = _commandHandlerFactory.GetHandler<T>(GetRepository, _connectionString);
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

        private IRepository GetRepository()
        {
            return _repository.Value;
        }
    }
}