using Microsoft.Extensions.DependencyInjection;
using Repository.Repository;
using Repository.Repository.Interfaces;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IWelcomeBannerRepository, WelcomeBannerRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ISpecialGameBannerRepository, SpecialGameBannerRepository>();

            return services;
        }
    }
}
