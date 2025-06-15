using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class PrivacyController : Controller
    {
        private readonly IPrivacyService _privacyService;
        public PrivacyController(IPrivacyService privacyService)
        {
            _privacyService = privacyService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _privacyService.GetAsync();
            return View(data);
        }
    }
}
