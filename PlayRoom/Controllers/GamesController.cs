using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        public GamesController(IGameService gameService, 
                               ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? category, string? priceRange, string? orderBy, int? page=1)
        {
            if (page == null) return BadRequest();

            ViewBag.SelectedCategory = category;
            ViewBag.SelectedPriceRange = priceRange;
            ViewBag.SelectedOrderBy = orderBy;

            if (page < 1) page = 1;
            var data = await _gameService.GetAllPaginated((int)page,16,category,priceRange,orderBy);

            var categories = await _categoryService.GetAllAsync();

            ViewBag.Category = categories;
            return View(data);
        }

    }
}
