namespace Pooka.Repo.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IDomain : IDisposable
    {
        Task<T> FindByIdAsync<T>(int id) where T : class;

        Task<T> FindSingleAsync<T>(QueryParameters<T> queryParameters) where T : class;

        Task<T[]> FindAsync<T>(QueryParameters<T> queryParameters) where T : class;

        Task<T[]> GetAllAsync<T>() where T : class;

        Task ExecuteAsync<T>(T command);         
    }
}