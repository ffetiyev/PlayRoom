using Service.ViewModels;

namespace Service.Service.Interfaces
{
    public interface IWelcomeBannerService
    {
        Task<WelcomeBannerVM> GetAsync();
    }
}
