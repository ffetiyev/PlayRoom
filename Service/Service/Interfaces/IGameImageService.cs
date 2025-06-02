using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interfaces
{
    public interface IGameImageService
    {
        Task<IEnumerable<GameImageVM>> GetAllAsync();
        Task<GameImageVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task SetMainImage(int id);
    }
}
