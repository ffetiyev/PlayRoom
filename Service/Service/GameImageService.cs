using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    public class GameImageService : IGameImageService
    {
        private readonly IGameImageRepository _gameImageRepo;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        public GameImageService(IGameImageRepository gameImageRepo,
                                IMapper mapper,
                                IGameRepository gameRepository)
        {
            _gameImageRepo = gameImageRepo;
            _mapper = mapper;
            _gameRepository = gameRepository;
        }

        public async Task DeleteAsync(int id)
        {
            await _gameImageRepo.DeleteAsync(await _gameImageRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<GameImageVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<GameImageVM>>(await _gameImageRepo.GetAllAsync());
        }

        public async Task<GameImageVM> GetByIdAsync(int id)
        {
          return _mapper.Map<GameImageVM>(await _gameImageRepo.GetByIdAsync(id));
        }

        public async Task SetMainImage(int id)
        {
            await _gameImageRepo.SetImageMain(id);

        }
    }
}
