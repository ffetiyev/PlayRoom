using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    public class PrivacyService : IPrivacyService
    {
        private readonly IPrivacyRepository _privacyRepository;
        public PrivacyService(IPrivacyRepository privacyRepository)
        {
            _privacyRepository = privacyRepository;
        }
        public async Task<PrivacyVM> GetAsync()
        {
            var data = await _privacyRepository.GetAllAsync();

            return data.Select(x => new PrivacyVM { Content = x.Content, Id = x.Id, Title = x.Title }).FirstOrDefault();
        }

        public async Task<PrivacyVM> GetByIdAsync(int id)
        {
            var data = await _privacyRepository.GetByIdAsync(id);
            return new PrivacyVM { Content = data.Content, Id = data.Id, Title = data.Title };
        }

        public async Task UpdateAsync(int id, PrivacyVM model)
        {
            var existData = await _privacyRepository.GetByIdAsync(id);
            if (model.Content != null) existData.Content = model.Content;
            if (model.Title != null) existData.Title = model.Title;
            await _privacyRepository.UpdateAsync(existData);
        }
    }
}
