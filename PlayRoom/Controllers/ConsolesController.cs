using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class ConsolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
