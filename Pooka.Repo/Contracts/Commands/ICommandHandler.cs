namespace Pooka.Repo.Contracts.Commands
{
    using System.Threading.Tasks;

    public interface ICommandHandler<in T>
    {
        Task ExecuteAsync(T command);
    }
}