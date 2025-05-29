using Service.ViewModels.SpecialGameBanner;

namespace Service.Service.Interfaces
{
    public interface ISpecialGameBannerService
    {
        Task<IEnumerable<SpecialGameBannerVM>> GetAllAsync();
        Task CreateAsync(SpecialGameBannerCreateVM model);
        Task<SpecialGameBannerVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id,SpecialGameBannerUpdateVM model);
        Task SetActiveBannerAsync(int id);
    }
}
