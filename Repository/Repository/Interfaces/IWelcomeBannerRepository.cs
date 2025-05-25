using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Interfaces
{
    public interface IWelcomeBannerRepository : IBaseRepository<WelcomeBanner>
    {
        Task<WelcomeBanner> GetAsync();
    }
}
