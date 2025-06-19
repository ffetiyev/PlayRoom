using Microsoft.AspNetCore.Mvc;
using Service.ViewModels;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;
using Service.ViewModels.SpecialGameBanner;
using Service.ViewModels.Game;
using Newtonsoft.Json;
using Service.ViewModels.Basket;
using Service.ViewModels.Favorites;
using Service.ViewModels.HomeShortcut;

namespace PlayRoom.Controllers;

public class HomeController : Controller
{

    private readonly IWelcomeBannerService _welcomeBannerService;
    private readonly IGameService _gameService;
    private readonly ISpecialGameBannerService _specialGameBannerService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHomeShortcutService _homeShortcutService;
    public HomeController(IWelcomeBannerService welcomeBannerService,
                          IGameService gameService,
                          ISpecialGameBannerService specialGameBannerService,
                          IHttpContextAccessor contextAccessor,
                          IHomeShortcutService homeShortcutService)
    {
        _welcomeBannerService = welcomeBannerService;
        _gameService = gameService;
        _specialGameBannerService = specialGameBannerService;
        _contextAccessor = contextAccessor;
        _homeShortcutService = homeShortcutService;
    }
    public async Task<IActionResult> Index()
    {
        WelcomeBannerVM welcomeBanners = await _welcomeBannerService.GetAsync();
        IEnumerable<GameVM> games = await _gameService.GetAllAsync();
        IEnumerable<SpecialGameBannerVM> specialGameBanners = await _specialGameBannerService.GetAllAsync();
        IEnumerable<HomeShortcutVM> homeShortcuts = await _homeShortcutService.GetAllAsync();

        HomeVM models = new()
        {
            WelcomeBanner=welcomeBanners,
            Games=games,
            SpecialGameBanners=specialGameBanners,
            HomeShortcuts=homeShortcuts
        };

        List<FavoritesVM> favoriteDatas = new();
        if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
        {
            favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
        }

        ViewBag.Favorites = favoriteDatas
            .Select(f => (f.ProductId, f.ProductType))
            .ToList();

        return View(models);
    }

    [HttpPost]
    public IActionResult AddProductToBasket(int id, string productType)
    {
        List<BasketVM> basketDatas = new List<BasketVM>();

        if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
        {
            basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
        }

        var existBasketDatas = basketDatas.FirstOrDefault(m => m.ProductType == productType && m.ProductCount == id);
        if (existBasketDatas == null)
        {

            basketDatas.Add(new() { ProductId = id, ProductCount = 1,ProductType=productType });
        }
        else
        {
            existBasketDatas.ProductCount++;
        }

        _contextAccessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));

        return Ok(basketDatas.Sum(m=>m.ProductCount));
    }

}
