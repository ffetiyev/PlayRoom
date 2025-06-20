using Domain.Models.Console;

namespace Repository.Repository.Interfaces
{
    public interface IConsoleRepository : IBaseRepository<Domain.Models.Console.Console>
    {
        Task AddCategoriesToConsole(int consoleId, IEnumerable<int> categoriIds);
        Task AddImagesToConsole(IEnumerable<ConsoleImage> images);
        IQueryable<Domain.Models.Console.Console> GetAllQueryable();
    }
}
