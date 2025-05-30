using AutoMapper;
using Domain.Models;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Category;

namespace Service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepo,
                               IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task CreateAsync(CategoryCreateVM model)
        {
            await _categoryRepo.CreateAsync(_mapper.Map<Category>(model));
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _categoryRepo.GetByIdAsync(id);
            await _categoryRepo.DeleteAsync(existData);
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryVM>>(await _categoryRepo.GetAllAsync());
        }

        public async Task<CategoryVM> GetByIdAsync(int id)
        {
           return _mapper.Map<CategoryVM>(await _categoryRepo.GetByIdAsync(id));   
        }

        public async Task UpdateAsync(int id,CategoryUpdateVM model)
        {
            var existData = await _categoryRepo.GetByIdAsync(id);
            if(model.Name != null) existData.Name = model.Name;
            await _categoryRepo.UpdateAsync(existData);
        }
    }
}
