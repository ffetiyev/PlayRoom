using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Setting;

namespace Service.Service
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public async Task<IEnumerable<SettingVM>> GetAllAsync()
        {
            var datas = await _settingRepository.GetAllAsync();
            return datas.Select(x => new SettingVM
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
            }).ToList();
        }

        public async Task<SettingVM> GetByIdAsync(int id)
        {
           var existData = await _settingRepository.GetByIdAsync(id);
            return new SettingVM { Id = existData.Id, Key = existData.Key,Value=existData.Value };
        }

        public async Task UpdateAsync(int id, SettingUpdateVM model)
        {
            var existData = await _settingRepository.GetByIdAsync(id);
            existData.Value = model.Value;
            await _settingRepository.UpdateAsync(existData);
        }
    }
}
