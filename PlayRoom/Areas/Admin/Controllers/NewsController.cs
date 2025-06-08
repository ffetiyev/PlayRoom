using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.News;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _env;
        public NewsController(INewsService newsService, 
                              IWebHostEnvironment env)
        {
            _newsService = newsService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _newsService.GetAllAsync();
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (request.Image.Length / 1024 > 2000)
            {
                ModelState.AddModelError("Image", "Image size should be smaller than 2 mb");
                return View(request);
            }
            if (!request.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "File type should be only image");
                return View(request);
            }
            
            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets","images","news",fileName);
            using(FileStream stream = new(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }
            request.ImageName = fileName;   
            await _newsService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            return View(existData);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            return View(new NewsUpdateVM { Description=existData.Description,Image=existData.Image,Title=existData.Title});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,NewsUpdateVM request)
        {
            if (id == null) return BadRequest();
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            if (!ModelState.IsValid) return View(request);

            if(request.NewImage != null)
            {
                if (request.Image.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("Image", "Image size should be smaller than 2 mb");
                    return View(request);
                }
                if (!request.NewImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Image", "File type should be only image");
                    return View(request);
                }

                string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "news", existData.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "news", fileName);
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
                request.Image = fileName;
            }

            await _newsService.UpdateAsync((int)id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
