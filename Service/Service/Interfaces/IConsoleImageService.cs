using Service.ViewModels.Console;

namespace Service.Service.Interfaces
{
    public interface IConsoleImageService
    {
        Task<IEnumerable<ConsoleImageVM>> GetAllAsync();
        Task<ConsoleImageVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task SetMainImage(int id);
    }
}
