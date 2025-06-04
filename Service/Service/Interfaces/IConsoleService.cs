using Service.Helpers.Responses;
using Service.ViewModels.Console;
using Service.ViewModels.Game;

namespace Service.Service.Interfaces
{
    public interface IConsoleService
    {
        Task<IEnumerable<ConsoleVM>> GetAllAsync();
        Task CreateAsync(ConsoleCreateVM model);
        Task<ConsoleVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, ConsoleUpdateVM model);
        Task<PaginateResponse<ConsoleVM>> GetAllPaginated(int page, int take = 16, string? category = null, string? priceRange = null, string? orderBy = null);
    }
}
