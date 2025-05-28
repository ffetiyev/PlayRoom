using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;

namespace Service.Service
{
    public class WelcomeBannerService : IWelcomeBannerService
    {
        private readonly IWelcomeBannerRepository _welcomeRepo;
        private readonly IMapper _mapper;
        public WelcomeBannerService(IWelcomeBannerRepository welcomeRepo,
                                    IMapper mapper)
        {
            _welcomeRepo = welcomeRepo;
            _mapper = mapper;
        }
        public async Task<WelcomeBannerVM> GetAsync()
        {
            return _mapper.Map<WelcomeBannerVM>(await _welcomeRepo.GetAsync());
        }

        public async Task UpdateAsync(int id, WelcomeBannerUpdateVM request, string? image)
        {
            var existData = await _welcomeRepo.GetAsync();
            if(image!=null) existData.Image = image;
            if (request.Title != null) existData.Title = request.Title;
            if (request.Description != null) existData.Description = request.Description;
            await _welcomeRepo.UpdateAsync(existData);
        }
    }
}
