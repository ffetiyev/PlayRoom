using Service.ViewModels.Accessory;

namespace Service.Service.Interfaces
{
    public interface IAccessoryImageService
    {
        Task<IEnumerable<AccessoryImageVM>> GetAllAsync();
        Task<AccessoryImageVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task SetMainImage(int id);
    }
}
