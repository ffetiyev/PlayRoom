using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, 
                                 ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
          var data = await _accountService.GetAllRoles();
            return View(data);
        }
        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRoles();
        //    return Ok();
        //}
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser(string userId)
        {
            if (userId == null)
            {
                _logger.LogInformation("Account/AddRoleToUser called at {Time}", DateTime.UtcNow);           
                return BadRequest();
            }

            var response = await _accountService.AddRoleToUser(userId);

            TempData["RoleMessage"] = response.Message;
            TempData["RoleSuccess"] = response.Success;
            _logger.LogError("Account/AddRoleToUser called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveAdminRoleFromUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("Account/RemoveAdminRoleFromUser called at {Time}", DateTime.UtcNow);
                return BadRequest();
            }

            var response = await _accountService.DeleteRoleFromUser(userId);

            TempData["RoleMessage"] = response.Message;
            TempData["RoleSuccess"] = response.Success;

            _logger.LogInformation("Account/RemoveAdminRoleFromUser called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }



    }
}
