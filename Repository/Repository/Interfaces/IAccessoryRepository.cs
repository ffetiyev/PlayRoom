using Domain.Models.Accessory;

namespace Repository.Repository.Interfaces
{
    public interface IAccessoryRepository : IBaseRepository<Accessory>
    {
        Task AddCategoriesToAccessory(int consoleId, IEnumerable<int> categoriIds);
        Task AddImagesToAccessory(IEnumerable<AccessoryImage> images);
        Task<IQueryable<Accessory>> GetAllQueryable();
    }
}
