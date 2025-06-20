using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class WelcomeBannerController : Controller
    {
        private readonly IWelcomeBannerService _welcomeBannerService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<WelcomeBannerController> _logger;
        public WelcomeBannerController(IWelcomeBannerService welcomeBannerService,
                                       IWebHostEnvironment env,
                                       ILogger<WelcomeBannerController> logger)
        {
            _welcomeBannerService = welcomeBannerService;
            _env = env;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            WelcomeBannerVM data = await _welcomeBannerService.GetAsync();
            _logger.LogInformation("WelcomeBanner/Index called at {Time}", DateTime.UtcNow);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _welcomeBannerService.GetAsync();
            if (existData == null) return NotFound();
            return View(new WelcomeBannerUpdateVM 
            { 
                Description=existData.Description,
                Title=existData.Title,
                Image=existData.Image,

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, WelcomeBannerUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("WelcomeBanner/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _welcomeBannerService.GetAsync();
            if (existData == null)
            {
                _logger.LogError("WelcomeBanner/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("WelcomeBanner/Update get error at {Time}", DateTime.UtcNow);
                request.Image=existData.Image;
                return View(request);
            }

            string? fileName = null;
            if (request.NewImage != null)
            {
                string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", existData.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                if (!request.NewImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("NewImage", "File type must be only image!");
                    return View(request);
                }

                if (request.NewImage.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("NewImage", "Picture length should be less than 2mb!");
                    return View(request);
                }

                fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
            }
            await _welcomeBannerService.UpdateAsync((int)id,request,fileName);
            _logger.LogInformation("WelcomeBanner/Index called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

    }
}
