using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    public class SpecialGameBannerService : ISpecialGameBannerService
    {
        private readonly ISpecialGameBannerRepository _specialGameBannerRepository;
        private readonly IMapper _mapper;
        public SpecialGameBannerService(ISpecialGameBannerRepository specialGameBannerRepository,
                                        IMapper mapper)
        {
            _specialGameBannerRepository = specialGameBannerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SpecialGameBannerVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SpecialGameBannerVM>>(await _specialGameBannerRepository.GetAllAsync());
        }
    }
}
