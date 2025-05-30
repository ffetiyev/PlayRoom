using Service.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyVM>> GetAllAsync();
        Task CreateAsync(CompanyCreateVM model);
        Task<CompanyVM> GetByIdAsync(int id);
        Task UpdateAsync(int id, CompanyUpdateVM model);
        Task DeleteAsync(int id);
    }
}
