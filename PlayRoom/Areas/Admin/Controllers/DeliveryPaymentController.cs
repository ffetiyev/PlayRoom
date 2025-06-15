using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeliveryPaymentController : Controller
    {
        private readonly IDeliveryPaymentService _service;
        public DeliveryPaymentController(IDeliveryPaymentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAsync();
            if (data == null) return NotFound();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _service.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,DeliveryPaymentVM request)
        {
            if (id == null) return BadRequest();
            var data = await _service.GetByIdAsync((int)id);
            if (data == null) return NotFound();
            if (!ModelState.IsValid) return View(request);
            await _service.UpdateAsync((int)id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
