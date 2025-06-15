using Service.ViewModels;

namespace Service.Service.Interfaces
{
    public interface IWarrantyService 
    {
        Task<WarrantyVM> GetAsync();
        Task UpdateAsync(int id, WarrantyVM model);
        Task<WarrantyVM> GetByIdAsync(int id);
    }
}
