using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Promocode;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class PromocodeController : Controller
    {
        private readonly IPromocodeService _promocodeService;
        private readonly ILogger<PromocodeController> _logger;
        public PromocodeController(IPromocodeService promocodeService, 
                                   ILogger<PromocodeController> logger)
        {
            _promocodeService = promocodeService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _promocodeService.GetAllAsync();
            _logger.LogInformation("Promocode/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromocodeCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _promocodeService.CreateAsync(request);
            _logger.LogInformation("HomeShortcut/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _promocodeService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(new PromocodeUpdateVM { Name = existData.Name, Value = existData.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, PromocodeUpdateVM request)
        {
            if (id == null) return BadRequest();
            var existData = await _promocodeService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            if (!ModelState.IsValid) return View(request);

            await _promocodeService.UpdateAsync((int)id, request);
            _logger.LogInformation("HomeShortcut/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _promocodeService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            await _promocodeService.DeleteAsync((int)id);
            _logger.LogInformation("HomeShortcut/Delete called at {Time}", DateTime.UtcNow);
            return Ok();
        }
    }
}