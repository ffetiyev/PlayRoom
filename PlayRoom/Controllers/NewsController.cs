using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
