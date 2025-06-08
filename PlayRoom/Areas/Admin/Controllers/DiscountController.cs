using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Discount;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
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
                return View(request);
            }
            if (request.Value > 100 || request.Value<0)
            {
                ModelState.AddModelError("Value", "Discount should be between 0 and 100");
                return View(request);
            }
            await _discountService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null) return NotFound();
            return View(new DiscountUpdateVM { Value=(int)existDiscount.Value});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,DiscountUpdateVM request)
        {
            if (id == null) return BadRequest();
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null) return NotFound();

            if (!ModelState.IsValid) return View(request);

            await _discountService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existDiscount = await _discountService.GetByIdAsync((int)id);
            if (existDiscount == null) return NotFound();
            await _discountService.DeleteAsync((int) id);
            return Ok();
        }
    }
}
