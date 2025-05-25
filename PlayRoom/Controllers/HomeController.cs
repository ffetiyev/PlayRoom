using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlayRoom.ViewModels;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers;

public class HomeController : Controller
{

    private readonly IWelcomeBannerService _welcomeBannerService;
    private readonly IMapper _mapper;
    public HomeController(IWelcomeBannerService welcomeBannerService,
                          IMapper mapper)
    {
        _mapper= mapper;
        _welcomeBannerService = welcomeBannerService;
    }
    public async Task<IActionResult> Index()
    {
        WelcomeBannerVM welcomeBanners = _mapper.Map<WelcomeBannerVM>(await _welcomeBannerService.GetAsync());
        HomeVM models = new()
        {
            WelcomeBanner=welcomeBanners
        };

        return View(models);
    }

}
