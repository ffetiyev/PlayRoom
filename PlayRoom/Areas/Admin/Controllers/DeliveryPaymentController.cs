using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DeliveryPaymentController : Controller
    {
        private readonly IDeliveryPaymentService _service;
        private readonly ILogger<DeliveryPaymentController> _logger;
        public DeliveryPaymentController(IDeliveryPaymentService service, 
                                         ILogger<DeliveryPaymentController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAsync();
            if (data == null)
            {
                _logger.LogError("Delivery/Index get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            _logger.LogInformation("Delivery/Index called at {Time}", DateTime.UtcNow);

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
            if (id == null)
            {
                _logger.LogError("Delivery/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var data = await _service.GetByIdAsync((int)id);
            if (data == null)
            {
                _logger.LogError("Delivery/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (!ModelState.IsValid) return View(request);
            await _service.UpdateAsync((int)id, request);
            _logger.LogInformation("Delivery/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
    }
}
