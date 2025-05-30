using Service.ViewModels.Category;

namespace Service.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();
        Task<CategoryVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id,CategoryUpdateVM model);
    }
}
