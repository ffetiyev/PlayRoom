using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        public async Task<IActionResult> Index()
        {
            var datas = await _newsService.GetAllAsync();
            return View(datas);
        }
    }
}
