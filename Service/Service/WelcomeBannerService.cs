using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

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
    }
}
