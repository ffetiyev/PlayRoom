using Microsoft.Extensions.DependencyInjection;
using Service.Service;
using Service.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IWelcomeBannerService, WelcomeBannerService>();
            return services;
        }
    }
}
