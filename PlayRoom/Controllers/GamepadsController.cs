using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class GamepadsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
