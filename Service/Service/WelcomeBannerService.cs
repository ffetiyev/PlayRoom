using Domain.Models;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class WelcomeBannerService : IWelcomeBannerService
    {
        private readonly IWelcomeBannerRepository _welcomeRepo;
        public WelcomeBannerService(IWelcomeBannerRepository welcomeRepo)
        {
            _welcomeRepo = welcomeRepo;
        }
        public async Task<WelcomeBanner> GetAsync()
        {
            return await _welcomeRepo.GetAsync();
        }
    }
}
