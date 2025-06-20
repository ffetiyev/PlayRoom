using Microsoft.AspNetCore.Http;
using Service.Helpers.Responses;
using Service.ViewModels.Account;

namespace Service.Service.Interfaces
{
    public interface IAccountService
    {
        //Task<RegisterResponse> Register(RegisterVM request);
        Task<RegisterResponse> RegisterAsync(RegisterVM request, HttpRequest httpRequest);
        Task<LoginResponse> Login(LoginVM request);
        Task Logout();
        Task CreateRoles();
        Task<IEnumerable<UserRolesDto>> GetAllRoles();
        Task<AddRoleRespone> AddRoleToUser(string userId);
        Task<DeleteRoleRespone> DeleteRoleFromUser(string userId);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<GenericResponse> ResetPasswordAsync(ResetPasswordVM model);
        Task<GenericResponse> ForgotPasswordAsync(string email, HttpRequest request);
    }
}
