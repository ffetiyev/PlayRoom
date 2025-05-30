using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.ViewModels;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;
using Service.ViewModels.SpecialGameBanner;
using Service.ViewModels.Game;

namespace PlayRoom.Controllers;

public class HomeController : Controller
{

    private readonly IWelcomeBannerService _welcomeBannerService;
    private readonly IGameService _gameService;
    private readonly ISpecialGameBannerService _specialGameBannerService;
    public HomeController(IWelcomeBannerService welcomeBannerService,
                          IGameService gameService,
                          ISpecialGameBannerService specialGameBannerService)
    {
        _welcomeBannerService = welcomeBannerService;
        _gameService = gameService;
        _specialGameBannerService = specialGameBannerService;
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

}
