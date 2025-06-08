
using Domain.Models;

namespace Repository.Repository.Interfaces
{
    public interface IConsoleRepository : IBaseRepository<Domain.Models.Console>
    {
        Task AddCategoriesToConsole(int consoleId, IEnumerable<int> categoriIds);
        Task AddImagesToConsole(IEnumerable<ConsoleImage> images);
        IQueryable<Domain.Models.Console> GetAllQueryable();
    }
}
