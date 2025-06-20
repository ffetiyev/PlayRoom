using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class PrivacyController : Controller
    {
        private readonly IPrivacyService _privacyService;
        private readonly ILogger<PrivacyController> _logger;
        public PrivacyController(IPrivacyService privacyService, 
                                ILogger<PrivacyController> logger)
        {
            _privacyService = privacyService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _privacyService.GetAsync();
            if (data == null)
            {
                _logger.LogError("Privacy/Index get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            _logger.LogInformation("Privacy/Index called at {Time}", DateTime.UtcNow);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _privacyService.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, PrivacyVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var data = await _privacyService.GetByIdAsync((int)id);
            if (data == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (!ModelState.IsValid) return View(request);
            await _privacyService.UpdateAsync((int)id, request);
            _logger.LogInformation("Privacy/Index called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
    }
}
