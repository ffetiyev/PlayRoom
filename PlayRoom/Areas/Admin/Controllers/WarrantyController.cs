using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class WarrantyController : Controller
    {
        private readonly IWarrantyService _warrantyService;
        private readonly ILogger<WarrantyController> _logger;
        public WarrantyController(IWarrantyService warrantyService, 
                                  ILogger<WarrantyController> logger)
        {
            _warrantyService = warrantyService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _warrantyService.GetAsync();
            if (data == null) return NotFound();
            _logger.LogInformation("Warranty/Index called at {Time}", DateTime.UtcNow);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _warrantyService.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, WarrantyVM request)
        {
            if (id == null)
            {
                _logger.LogError("Warranty/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var data = await _warrantyService.GetByIdAsync((int)id);
            if (data == null)
            {
                _logger.LogError("Warranty/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Warranty/Update get error at {Time}", DateTime.UtcNow);
                return View(request);
            }
            await _warrantyService.UpdateAsync((int)id, request);
            _logger.LogInformation("Warranty/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
    }
}
