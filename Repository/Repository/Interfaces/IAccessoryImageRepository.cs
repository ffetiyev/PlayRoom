using Domain.Models.Accessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Interfaces
{
    public interface IAccessoryImageRepository : IBaseRepository<AccessoryImage>
    {
        Task SetImageMain(int id);
    }
}
