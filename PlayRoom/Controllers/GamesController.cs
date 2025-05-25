using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
