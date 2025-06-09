using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.News;
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
            var count = datas.Count();  
            ViewBag.Count = count;
            return View(datas.Take(8));
        }
        public async Task<IActionResult> ShowMore(int skip)
        {
            var news = await _newsService.GetAllAsync();

            var datas =news.Skip(skip).Take(4).ToList();

            return PartialView("_NewsPartial",datas);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            var datas = await _newsService.GetAllAsync();
            var filteredDatas = datas.TakeLast(3).ToList(); 
            NewsDetailVM model = new()
            {
                ExistNews = existData,
                LatestNews=filteredDatas,
            };
            return View(model);
        }
    }
}
