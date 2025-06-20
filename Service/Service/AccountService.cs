using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Helpers.Responses;
using Service.Helpers.Roles;
using Service.Service.Interfaces;
using Service.ViewModels.Account;

namespace Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        public AccountService(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<AddRoleRespone> AddRoleToUser(string userId)
        {
            var response = new AddRoleRespone();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(Roles.Admin.ToString()))
            {
                response.Success = false;
                response.Message = "User already has Admin role.";
                return response;
            }

            var result = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "Admin role successfully added.";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to add role.";
            }
            return response;
        }

        public async Task CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
        }

        public async Task<DeleteRoleRespone> DeleteRoleFromUser(string userId)
        {
            var response = new DeleteRoleRespone();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(Roles.Admin.ToString()))
            {
                response.Success = false;
                response.Message = "User doesn't have Admin role.";
                return response;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, Roles.Admin.ToString());
            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "Admin role successfully removed.";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to remove Admin role.";
            }

            return response;
        }

        public async Task<IEnumerable<UserRolesDto>> GetAllRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserRolesDto> userWithRoles = new List<UserRolesDto>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Id = user.Id,
                    Roles = userRoles.ToList()
                });
            }
            return userWithRoles;
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

        //public async Task<RegisterResponse> Register(RegisterVM request)
        //{
        //    AppUser user = new()
        //    {
        //        FullName = request.FullName,
        //        Email = request.Email,
        //        PhoneNumber = request.PhoneNumber,
        //        Address = request.Address,
        //        City = request.City,
        //        UserName = request.Username,
        //    };
        //    var result = await _userManager.CreateAsync(user, request.Password);

        //    if (!result.Succeeded)
        //    {
        //        return new RegisterResponse { Success = false, Errors = result.Errors.Select(m => m.Description).ToArray() };

        //    }

        //    await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

        //    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        //    await _signInManager.SignInAsync(user, false);

        //    return new RegisterResponse { Success = true, Errors = null };
        //}
        public async Task<RegisterResponse> RegisterAsync(RegisterVM request, HttpRequest httpRequest)
        {
            AppUser newUser = new()
            {
                FullName = request.FullName,
                UserName = request.Username,
                Email = request.Email,
                Address=request.Address,
                City=request.City,
                PhoneNumber=request.PhoneNumber
            };

            var createResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createResult.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Errors = createResult.Errors.Select(e => e.Description).ToArray()
                };
            }

            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            string url = UrlHelper(httpRequest, "Account", "ConfirmEmail", new { userId = newUser.Id, token });

            string html;
            using (StreamReader sr = new StreamReader("wwwroot/templates/emailConfirm.html"))
            {
                html = await sr.ReadToEndAsync();
            }

            html = html.Replace("{link-unique}", url);
            _emailService.Send(newUser.Email, "Email confirmation for account.", html);

            return new RegisterResponse
            {
                Success = true,
                Errors = Array.Empty<string>()
            };
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        private string UrlHelper(HttpRequest request, string controller, string action, object routeValues)
        {
            var scheme = request.Scheme;
            var host = request.Host.ToString();

            var queryDict = routeValues
                .GetType()
                .GetProperties()
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(routeValues)?.ToString() ?? string.Empty
                );

            var urlBuilder = new UriBuilder($"{scheme}://{host}")
            {
                Path = $"/{controller}/{action}",
                Query = QueryString.Create(queryDict).ToUriComponent()
            };

            return urlBuilder.ToString();
        }

        public async Task<GenericResponse> ForgotPasswordAsync(string email, HttpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new GenericResponse { Success = false, Message = "Email tapılmadı və ya təsdiqlənməyib." };
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = UrlHelper(request, "Account", "ResetPassword", new { userId = user.Id, token });

            string html;
            using (StreamReader sr = new StreamReader("wwwroot/templates/resetPassword.html"))
            {
                html = await sr.ReadToEndAsync();
            }

            html = html.Replace("{link-unique}", url);

            _emailService.Send(user.Email, "Reset your password", html);

            return new GenericResponse { Success = true };
        }

        public async Task<GenericResponse> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new GenericResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
            {
                return new GenericResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new GenericResponse { Success = true };
        }


    }
}
