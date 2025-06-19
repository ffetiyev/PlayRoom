using Microsoft.AspNetCore.Http;
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
            services.AddScoped<IConsoleService, ConsoleService>();
            services.AddScoped<IConsoleImageService, ConsoleImageService>();
            services.AddScoped<IAccessoryService, AccessoryService>();
            services.AddScoped<IAccessoryImageService, AccessoryImageService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IDeliveryPaymentService, DeliveryPaymentService>();
            services.AddScoped<IWarrantyService, WarrantyService>();
            services.AddScoped<IPrivacyService, PrivacyService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IHomeShortcutService, HomeShortcutService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
