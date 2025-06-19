using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.HomeShortcut;

namespace Service.Service
{
    public class HomeShortcutService : IHomeShortcutService
    {
        private readonly IHomeShortcutRepository _repository;
        public HomeShortcutService(IHomeShortcutRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<HomeShortcutVM>> GetAllAsync()
        {
           var datas = await _repository.GetAllAsync();
            return datas.Select(x => new HomeShortcutVM {Id=x.Id,Description=x.Description,Image=x.Image,Title=x.Title,Type=x.Type});
        }

        public async Task<HomeShortcutVM> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return new HomeShortcutVM { Id=data.Id, Description=data.Description,Image=data.Image,Title=data.Title,Type=data.Type};    
        }

        public async Task UpdateAsync(int id, HomeShortcutUpdateVM model)
        {
            var data = await _repository.GetByIdAsync(id);
            if(model.Title != null) data.Title = model.Title;
            if(model.Description!=null) data.Description = model.Description;
            if(model.Image!=null) data.Image = model.Image;
            await _repository.UpdateAsync(data);
        }
    }
}
