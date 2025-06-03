using Service.ViewModels.Console;

namespace Service.Service.Interfaces
{
    public interface IConsoleService
    {
        Task<IEnumerable<ConsoleVM>> GetAllAsync();
        Task CreateAsync(ConsoleCreateVM model);
        Task<ConsoleVM> GetByIdAsync(int id);
    }
}
