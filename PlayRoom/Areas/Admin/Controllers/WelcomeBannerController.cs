using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WelcomeBannerController : Controller
    {
        private readonly IWelcomeBannerService _welcomeBannerService;
        public WelcomeBannerController(IWelcomeBannerService welcomeBannerService)
        {
            _welcomeBannerService = welcomeBannerService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            WelcomeBannerVM data = await _welcomeBannerService.GetAsync();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            return View();
        }

    }
}
