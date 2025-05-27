using Service.ViewModels;

namespace Service.Service.Interfaces
{
    public interface ISpecialGameBannerService
    {
        Task<IEnumerable<SpecialGameBannerVM>> GetAllAsync();
    }
}
