using Azure.Core;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Responses;
using Service.Service.Interfaces;
using Service.ViewModels.Account;

namespace PlayRoom.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM request)
        {
            if (!ModelState.IsValid) return View(request);

            RegisterResponse response = await _accountService.Register(request);

            if (!response.Success)
            {
                foreach (var error in response.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(request);
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM request)
        {
            if (!ModelState.IsValid) return View(request);

            LoginResponse response = await _accountService.Login(request);
            if (!response.Success)
            {
                    ModelState.AddModelError(string.Empty, response.Message);

                return View(request);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
