using Domain.Models;

namespace Repository.Repository.Interfaces
{
    public interface IWelcomeBannerRepository : IBaseRepository<WelcomeBanner>
    {
        Task<WelcomeBanner> GetAsync();
    }
}
