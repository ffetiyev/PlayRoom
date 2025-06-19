using Service.Helpers.Responses;
using Service.ViewModels.Account;

namespace Service.Service.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponse> Register(RegisterVM request);
        Task<LoginResponse> Login(LoginVM request);
        Task Logout();
    }
}
