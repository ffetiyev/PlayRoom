using Service.Helpers.Responses;
using Service.ViewModels.Accessory;
using Service.ViewModels.Console;


namespace Service.Service.Interfaces
{
    public interface IAccessoryService
    {
        Task<IEnumerable<AccessoryVM>> GetAllAsync();
        Task CreateAsync(AccessoryCreateVM model);
        Task<AccessoryVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, AccessoryUpdateVM model);
        Task<PaginateResponse<AccessoryVM>> GetAllPaginated(int page, int take = 8, string? category = null, string? priceRange = null, string? orderBy = null);
    }
}
