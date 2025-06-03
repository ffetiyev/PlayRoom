
namespace Repository.Repository.Interfaces
{
    public interface IConsoleRepository : IBaseRepository<Domain.Models.Console>
    {
        Task AddCategoriesToConsole(int consoleId, IEnumerable<int> categoriIds);
    }
}
