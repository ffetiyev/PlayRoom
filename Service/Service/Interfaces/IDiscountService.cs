using Service.ViewModels.Discount;

namespace Service.Service.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountVM>> GetAllAsync();
        Task CreateAsync(DiscountCreateVM request);
        Task<DiscountVM> GetByIdAsync(int id);
        Task UpdateAsync(int id, DiscountUpdateVM model);
        Task DeleteAsync(int id);
    }
}
