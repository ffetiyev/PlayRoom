using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryService _accessoryService;
        private readonly ICategoryService _categoryService;
        public AccessoriesController(IAccessoryService accessoryService,
                               ICategoryService categoryService)
        {
            _accessoryService = accessoryService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? category, string? priceRange, string? orderBy, int? page = 1)
        {
            if (page == null) return BadRequest();

            ViewBag.SelectedCategory = category;
            ViewBag.SelectedPriceRange = priceRange;
            ViewBag.SelectedOrderBy = orderBy;

            if (page < 1) page = 1;
            var data = await _accessoryService.GetAllPaginated((int)page, 8, category, priceRange, orderBy);

            var categories = await _categoryService.GetAllAsync();

            ViewBag.Category = categories;
            return View(data);
        }
    }
}
