using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Discount;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;
        public DiscountController(IDiscountService discountService, 
                                  ILogger<DiscountController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Discount/Index called at {Time}", DateTime.UtcNow);
            return View(await _discountService.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Discount/Create get error at {Time}", DateTime.UtcNow);
                return View(request);
            }
            if (request.Value > 100 || request.Value<0)
            {
                ModelState.AddModelError("Value", "Discount should be between 0 and 100");
                return View(request);
            }
            await _discountService.CreateAsync(request);
            _logger.LogInformation("Discount/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null) return NotFound();
            _logger.LogInformation("Discount/Create called at {Time}", DateTime.UtcNow);
            return View(new DiscountUpdateVM { Value=(int)existDiscount.Value});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,DiscountUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return View(request);
            }

            await _discountService.UpdateAsync((int)id, request);
            _logger.LogInformation("Discount/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            await _discountService.DeleteAsync((int) id);
            _logger.LogInformation("Discount/Delete called at {Time}", DateTime.UtcNow);
            return Ok();
        }
    }
}
