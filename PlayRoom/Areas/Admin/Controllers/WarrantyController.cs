using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarrantyController : Controller
    {
        private readonly IWarrantyService _warrantyService;
        public WarrantyController(IWarrantyService warrantyService)
        {
            _warrantyService = warrantyService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _warrantyService.GetAsync();
            if (data == null) return NotFound();
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
            if (id == null) return BadRequest();
            var data = await _warrantyService.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            if (!ModelState.IsValid) return View(request);
            await _warrantyService.UpdateAsync((int)id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
