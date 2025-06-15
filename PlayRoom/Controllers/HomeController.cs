using Microsoft.AspNetCore.Mvc;
using Service.ViewModels;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;
using Service.ViewModels.SpecialGameBanner;
using Service.ViewModels.Game;
using Newtonsoft.Json;
using Service.ViewModels.Basket;

namespace PlayRoom.Controllers;

public class HomeController : Controller
{

    private readonly IWelcomeBannerService _welcomeBannerService;
    private readonly IGameService _gameService;
    private readonly ISpecialGameBannerService _specialGameBannerService;
    private readonly IHttpContextAccessor _contextAccessor;
    public HomeController(IWelcomeBannerService welcomeBannerService,
                          IGameService gameService,
                          ISpecialGameBannerService specialGameBannerService,
                          IHttpContextAccessor contextAccessor)
    {
        _welcomeBannerService = welcomeBannerService;
        _gameService = gameService;
        _specialGameBannerService = specialGameBannerService;
        _contextAccessor = contextAccessor;
    }
    public async Task<IActionResult> Index()
    {
        WelcomeBannerVM welcomeBanners = await _welcomeBannerService.GetAsync();
        IEnumerable<GameVM> games = await _gameService.GetAllAsync();
        IEnumerable<SpecialGameBannerVM> specialGameBanners = await _specialGameBannerService.GetAllAsync();

        HomeVM models = new()
        {
            WelcomeBanner=welcomeBanners,
            Games=games,
            SpecialGameBanners=specialGameBanners
        };

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

        var existData = basketDatas.FirstOrDefault(m=>m.ProductId==id);
        if (existData == null)
        {

            basketDatas.Add(new() { ProductId = id, ProductCount = 1,ProductType=productType });
        }
        else
        {
            existData.ProductCount++;
        }

        _contextAccessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));

        return Ok(basketDatas.Sum(m=>m.ProductCount));
    }

}
