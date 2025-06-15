using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
