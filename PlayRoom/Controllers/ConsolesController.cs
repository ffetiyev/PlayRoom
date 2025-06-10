using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class ConsolesController : Controller
    {
        private readonly IConsoleService _consoleService;
        private readonly ICategoryService _categoryService;
        public ConsolesController(IConsoleService consoleService,ICategoryService categoryService)
        {
            _consoleService = consoleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, string? priceRange, string? orderBy, int? page = 1)
        {
            if (page == null) return BadRequest();

            ViewBag.SelectedCategory = category;
            ViewBag.SelectedPriceRange = priceRange;
            ViewBag.SelectedOrderBy = orderBy;

            if (page < 1) page = 1;
            var data = await _consoleService.GetAllPaginated((int)page, 8, category, priceRange, orderBy);

            var categories = await _categoryService.GetAllAsync();

            ViewBag.Category = categories;
            return View(data);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _consoleService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
    }
}
