using Service.ViewModels;


namespace Service.Service.Interfaces
{
    public interface IPrivacyService 
    {
        Task<PrivacyVM> GetAsync();
        Task UpdateAsync(int id, PrivacyVM model);
        Task<PrivacyVM> GetByIdAsync(int id);
    }
}
