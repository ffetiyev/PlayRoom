using AutoMapper;
using Domain.Models;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.SpecialGameBanner;

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

        public async Task CreateAsync(SpecialGameBannerCreateVM model)
        {
            await _specialGameBannerRepository.CreateAsync(_mapper.Map<SpecialGameBanner>(model));
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _specialGameBannerRepository.GetByIdAsync(id);
            await _specialGameBannerRepository.DeleteAsync(existData);
        }

        public async Task SetActiveBannerAsync(int id)
        {
            await _specialGameBannerRepository.SetActiveBannerAsync(id);
        }

        public async Task<IEnumerable<SpecialGameBannerVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SpecialGameBannerVM>>(await _specialGameBannerRepository.GetAllAsync());
        }

        public async Task<SpecialGameBannerVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SpecialGameBannerVM>(await _specialGameBannerRepository.GetByIdAsync(id));
        }

        public async Task UpdateAsync(int id,SpecialGameBannerUpdateVM model)
        {
            var existData = await _specialGameBannerRepository.GetByIdAsync(id);
            if(model.Name != null) existData.Name = model.Name;
            if (model.Description != null) existData.Description = model.Description;
            if (model.Image != null) existData.Image = model.Image;
            await _specialGameBannerRepository.UpdateAsync(existData);
        }
    }
}
