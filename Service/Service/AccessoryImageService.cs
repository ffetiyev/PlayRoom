using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Game;

namespace Service.Service
{
    public class AccessoryImageService : IAccessoryImageService
    {
        private readonly IAccessoryImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public AccessoryImageService(IAccessoryImageRepository imageRepository,
                                     IMapper mapper)
        {
            _imageRepository = imageRepository;   
            _mapper = mapper;
        }
        public async Task DeleteAsync(int id)
        {
           await _imageRepository.DeleteAsync(await _imageRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<AccessoryImageVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AccessoryImageVM>>(await _imageRepository.GetAllAsync());
        }

        public async Task<AccessoryImageVM> GetByIdAsync(int id)
        {
            return _mapper.Map<AccessoryImageVM>(await _imageRepository.GetByIdAsync(id));
        }

        public async Task SetMainImage(int id)
        {
          await _imageRepository.SetImageMain(id);
        }
    }
}
