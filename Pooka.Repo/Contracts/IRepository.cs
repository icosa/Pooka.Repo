namespace Pooka.Repo.Contracts
{
    using System.Threading.Tasks;

    using Queries;

    public interface IRepository
    {
        Task<T> FindByIdAsync<T>(int id) where T : class;

        Task<T> FindAsync<T>(ISingleEntityQuery<T> query) where T : class;

        Task<T[]> FindAsync<T>(IEntityQuery<T> query) where T : class;

        Task<T[]> GetAllAsync<T>() where T : class;

        Task<T> AddAsync<T>(T item, bool commit = false) where T : class;

        Task RemoveAsync<T>(T item, bool commit = false) where T : class;

        Task<int> SaveChangesAsync();
    }
}