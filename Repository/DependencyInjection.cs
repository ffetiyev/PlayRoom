﻿using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGameImageRepository, GameImageRepository>();
            services.AddScoped<IConsoleRepository, ConsoleRepository>();
            services.AddScoped<IConsoleImageRepository, ConsoleImageRepository>();

            return services;
        }
    }
}
