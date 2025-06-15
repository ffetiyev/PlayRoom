using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IWarrantyRepository _repository;
        public WarrantyService(IWarrantyRepository repository)
        {
            _repository = repository;
        }
        public async Task<WarrantyVM> GetAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(x => new WarrantyVM { Content = x.Content, Id = x.Id,Title=x.Title}).FirstOrDefault();
        }

        public async Task<WarrantyVM> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return new WarrantyVM { Content=data.Content, Id = data.Id,Title=data.Title };
        }

        public async Task UpdateAsync(int id, WarrantyVM model)
        {
            var existData = await _repository.GetByIdAsync(id);
            if (model.Content != null) existData.Content = model.Content;
            if (model.Title != null) existData.Title = model.Title;
            await _repository.UpdateAsync(existData);
        }
    }
}
