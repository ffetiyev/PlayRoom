using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Setting;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IActionResult> Index()
        {
            var datas = await _settingService.GetAllAsync();
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
            if (id == null) return BadRequest();
            var existData = await _settingService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            await _settingService.UpdateAsync((int)id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
