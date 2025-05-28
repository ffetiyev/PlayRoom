using Service.ViewModels.WelcomeBanner;

namespace Service.Service.Interfaces
{
    public interface IWelcomeBannerService
    {
        Task<WelcomeBannerVM> GetAsync();
        Task UpdateAsync(int id, WelcomeBannerUpdateVM request,string? image);
    }
}
