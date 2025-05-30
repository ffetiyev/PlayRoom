using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Company;

namespace Service.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        public CompanyService(ICompanyRepository companyRepository,
                              IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CompanyCreateVM model)
        {
            await _companyRepository.CreateAsync(new() { Name = model.Name, Image = model.Image });
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _companyRepository.GetByIdAsync(id);
            await _companyRepository.DeleteAsync(existData);
        }

        public async Task<IEnumerable<CompanyVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CompanyVM>>(await _companyRepository.GetAllAsync());
        }

        public async Task<CompanyVM> GetByIdAsync(int id)
        {
           return _mapper.Map<CompanyVM>(await _companyRepository.GetByIdAsync(id));
        }

        public async Task UpdateAsync(int id, CompanyUpdateVM model)
        {
            var existData = await _companyRepository.GetByIdAsync(id);
            if ((model.Image != null)) existData.Image = model.Image;
            if (model.Name != null) existData.Name = model.Name;
            await _companyRepository.UpdateAsync(existData);
        }
    }
}
