using Service.ViewModels.HomeShortcut;

namespace Service.Service.Interfaces
{
    public interface IHomeShortcutService
    {
        Task<IEnumerable<HomeShortcutVM>> GetAllAsync();
        Task<HomeShortcutVM> GetByIdAsync(int id);
        Task UpdateAsync(int id, HomeShortcutUpdateVM model);
    }
}
