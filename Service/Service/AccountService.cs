using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Service.Helpers.Responses;
using Service.Service.Interfaces;
using Service.ViewModels.Account;

namespace Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountService(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResponse> Login(LoginVM request)
        {
            var user = await _userManager.FindByNameAsync(request.UsernameOrEmail)
                     ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "İstifadəçi tapılmadı və ya məlumatlar yanlışdır."
                };
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Şifrə yanlışdır və ya istifadəçi deaktivdir."
                };
            }
            await _signInManager.SignInAsync(user, isPersistent: false);

            return new LoginResponse
            {
                Success = true,
                Username = user.UserName
            };
        }



        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> Register(RegisterVM request)
        {
            AppUser user = new()
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                UserName = request.Username,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse { Success = false, Errors = result.Errors.Select(m => m.Description).ToArray() };

            }

            await _signInManager.SignInAsync(user, false);
            
            return new RegisterResponse { Success = true, Errors = null };
        }
    }
}
