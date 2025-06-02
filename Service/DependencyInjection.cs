using Microsoft.Extensions.DependencyInjection;
using Service.Service;
using Service.Service.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IWelcomeBannerService, WelcomeBannerService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ISpecialGameBannerService, SpecialGameBannerService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IGameImageService, GameImageService>();
            return services;
        }
    }
}
