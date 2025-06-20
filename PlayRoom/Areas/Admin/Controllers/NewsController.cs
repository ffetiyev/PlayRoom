using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.News;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<NewsController> _logger;
        public NewsController(INewsService newsService, 
                              IWebHostEnvironment env,
                              ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _env = env;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _newsService.GetAllAsync();
            _logger.LogInformation("News/Index called at {Time}", DateTime.UtcNow);

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

            if (!string.IsNullOrEmpty(request.VideoLink) && !request.VideoLink.Contains("watch?v"))
            {
                ModelState.AddModelError("VideoLink", "Link type is wrong, it should contains 'watch?v'!");
                return View(request);
            }

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
            _logger.LogInformation("News/Index called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            _logger.LogInformation("News/Detail called at {Time}", DateTime.UtcNow);
            return View(existData);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            return View(new NewsUpdateVM { Description=existData.Description,Image=existData.Image,Title=existData.Title,VideoLink=existData.VideoLink});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,NewsUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            ;

            if (!ModelState.IsValid)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return View(request);
            }

            if (!string.IsNullOrEmpty(request.VideoLink) && !request.VideoLink.Contains("watch?v"))
            {
                ModelState.AddModelError("VideoLink", "Link type is wrong, it should contains 'watch?v'!");
                return View(request);
            }

            if (request.NewImage != null)
            {
                if (request.NewImage.Length / 1024 > 2000)
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
            _logger.LogInformation("News/Index called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _newsService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "news", existData.Image);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            await _newsService.DeleteAsync((int)id);
            _logger.LogInformation("News/Index called at {Time}", DateTime.UtcNow);
            return Ok();
        }
    }
}
