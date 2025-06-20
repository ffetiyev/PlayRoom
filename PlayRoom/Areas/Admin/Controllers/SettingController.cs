using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Setting;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly ILogger<SettingController> _logger;
        public SettingController(ISettingService settingService, 
                                 ILogger<SettingController> logger)
        {
            _settingService = settingService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var datas = await _settingService.GetAllAsync();
            _logger.LogInformation("Setting/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _settingService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(new SettingUpdateVM { Key=existData.Key,Value=existData.Value});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SettingUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _settingService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            await _settingService.UpdateAsync((int)id, request);
            _logger.LogInformation("Setting/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
    }
}
