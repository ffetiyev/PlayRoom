using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Console;
using Service.ViewModels.Favorites;
using Service.ViewModels.Game;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGameService _gameService;
        private readonly IConsoleService _consoleService;
        private readonly IAccessoryService _accessoryService;
        public FavoritesController(IHttpContextAccessor contextAccessor,
                                   IGameService gameService,
                                   IConsoleService consoleService,
                                   IAccessoryService accessoryService)
        {
            _contextAccessor = contextAccessor;
            _gameService = gameService;
            _consoleService = consoleService;
            _accessoryService = accessoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            ViewBag.Favorites = favoriteDatas
                   .Select(f => (f.ProductId, f.ProductType))
                   .ToList();

            List<GameVM> games = new();

            foreach (var item in favoriteDatas.Where(m=>m.ProductType=="game"))
            {
                var existData = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(existData);
            }


            List<ConsoleVM> consoles = new();

            foreach (var item in favoriteDatas.Where(m => m.ProductType == "console"))
            {
                var existData = await _consoleService.GetByIdAsync(item.ProductId);
                consoles.Add(existData);
            }


            List<AccessoryVM> accessories = new();

            foreach (var item in favoriteDatas.Where(m => m.ProductType == "accessory"))
            {
                var existData = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(existData);
            }

            return View(new FavoriteDetailVM { Accessories=accessories,Consoles=consoles,Games=games});
        }
        public IActionResult AddToFavorites(int id,string type)
        {
            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"]!=null)
            {
                favoriteDatas=JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            var favoriteData = favoriteDatas.FirstOrDefault(m=>m.ProductId==id && m.ProductType==type);
            if (favoriteData==null)
            {
                favoriteDatas.Add(new FavoritesVM() { ProductId=id,ProductType=type});
            }

            _contextAccessor.HttpContext.Response.Cookies.Append("favorites",JsonConvert.SerializeObject(favoriteDatas));

                return Ok(favoriteDatas.Count());
        }
        public IActionResult DeleteFromFavorites(int id, string type)
        {
            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            var favoriteData = favoriteDatas.FirstOrDefault(m => m.ProductId == id && m.ProductType == type);
            if (favoriteData != null)
            {
                favoriteDatas.Remove(favoriteData);
            }

            _contextAccessor.HttpContext.Response.Cookies.Append("favorites", JsonConvert.SerializeObject(favoriteDatas));

            return Ok(favoriteDatas.Count());
        }
    }
}
