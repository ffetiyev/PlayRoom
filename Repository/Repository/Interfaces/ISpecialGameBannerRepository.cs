using Domain.Models;

namespace Repository.Repository.Interfaces
{
    public interface ISpecialGameBannerRepository : IBaseRepository<SpecialGameBanner>
    {
        Task SetActiveBannerAsync(int id);
    }
}
