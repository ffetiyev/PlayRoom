using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrivacyController : Controller
    {
        private readonly IPrivacyService _privacyService;
        public PrivacyController(IPrivacyService privacyService)
        {
            _privacyService = privacyService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _privacyService.GetAsync();
            if (data == null) return NotFound();
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
            if (id == null) return BadRequest();
            var data = await _privacyService.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            if (!ModelState.IsValid) return View(request);
            await _privacyService.UpdateAsync((int)id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
