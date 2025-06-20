using Service.ViewModels.Promocode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interfaces
{
   public interface IPromocodeService
    {
        Task<IEnumerable<PromocodeVM>> GetAllAsync();
        Task<PromocodeVM> GetByIdAsync(int id);
        Task CreateAsync(PromocodeCreateVM model);
        Task UpdateAsync(int id, PromocodeUpdateVM model);
        Task DeleteAsync(int id);
    }
}
