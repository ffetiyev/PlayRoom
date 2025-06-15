using Service.ViewModels.Setting;

namespace Service.Service.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingVM>> GetAllAsync();
        Task UpdateAsync(int id,SettingUpdateVM model);
        Task<SettingVM> GetByIdAsync(int id);
    }
}
