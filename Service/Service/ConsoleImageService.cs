using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Console;

namespace Service.Service
{
    public class ConsoleImageService : IConsoleImageService
    {
        private readonly IConsoleImageRepository _consoleImageRepo;
        private readonly IMapper _mapper;
        public ConsoleImageService(IConsoleImageRepository consoleImageRepo,IMapper mapper)
        {
            _consoleImageRepo = consoleImageRepo;
            _mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {
            await _consoleImageRepo.DeleteAsync(await _consoleImageRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<ConsoleImageVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ConsoleImageVM>>(await _consoleImageRepo.GetAllAsync());
        }

        public async Task<ConsoleImageVM> GetByIdAsync(int id)
        {
            return _mapper.Map<ConsoleImageVM>(await _consoleImageRepo.GetByIdAsync(id));
        }

        public async Task SetMainImage(int id)
        {
            await _consoleImageRepo.SetImageMain(id);
        }
    }
}
