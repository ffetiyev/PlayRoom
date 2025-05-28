using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.WelcomeBanner;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WelcomeBannerController : Controller
    {
        private readonly IWelcomeBannerService _welcomeBannerService;
        private readonly IWebHostEnvironment _env;
        public WelcomeBannerController(IWelcomeBannerService welcomeBannerService,
                                       IWebHostEnvironment env)
        {
            _welcomeBannerService = welcomeBannerService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            WelcomeBannerVM data = await _welcomeBannerService.GetAsync();
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
            if (id == null) return BadRequest();
            var existData = await _welcomeBannerService.GetAsync();
            if (existData == null) return NotFound();
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

            return RedirectToAction(nameof(Index));
        }

    }
}
