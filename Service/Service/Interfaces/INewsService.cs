using Service.ViewModels.News;

namespace Service.Service.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsVM>> GetAllAsync();
        Task<NewsVM> GetByIdAsync(int id);
        Task CreateAsync(NewsCreateVM model);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, NewsUpdateVM model);
    }
}
