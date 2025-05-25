using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Controllers
{
    public class AccessoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
