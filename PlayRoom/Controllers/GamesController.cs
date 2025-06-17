using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Service.Interfaces;
using Service.ViewModels.Favorites;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;
        public GamesController(IGameService gameService, 
                               ICategoryService categoryService,
                               IHttpContextAccessor contextAccessor)
        {
            _gameService = gameService;
            _categoryService = categoryService;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? category, string? priceRange, string? orderBy, int? page = 1)
        {
            if (page == null) return BadRequest();

            ViewBag.SelectedCategory = category;
            ViewBag.SelectedPriceRange = priceRange;
            ViewBag.SelectedOrderBy = orderBy;

            if (page < 1) page = 1;
            var data = await _gameService.GetAllPaginated((int)page, 8, category, priceRange, orderBy);

            var categories = await _categoryService.GetAllAsync();

            ViewBag.Category = categories;

            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            ViewBag.Favorites = favoriteDatas
                .Select(f => (f.ProductId, f.ProductType))
                .ToList();


            return View(data);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            ViewBag.Favorites = favoriteDatas
                .Select(f => (f.ProductId, f.ProductType))
                .ToList();
            return View(existData);
        }

    }
}
