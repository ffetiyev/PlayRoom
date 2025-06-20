using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.SpecialGameBanner;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SpecialGameBannerController : Controller
    {
        private readonly ISpecialGameBannerService _specialGameService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SpecialGameBannerController> _logger;
        public SpecialGameBannerController(ISpecialGameBannerService specialGameService,
                                           IWebHostEnvironment env,
                                           ILogger<SpecialGameBannerController> logger)
        {
            _specialGameService = specialGameService;
            _env = env;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _specialGameService.GetAllAsync();
            _logger.LogInformation("SpecialGameBannerController/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialGameBannerCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("SpecialGameBannerController/Create get error at {Time}", DateTime.UtcNow);
                return View(request);
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await request.NewImage.CopyToAsync(stream);
            }
            request.Image = fileName;
            request.IsActive = false;
            await _specialGameService.CreateAsync(request);
            _logger.LogInformation("SpecialGameBannerController/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("SetActiveBanner/Delete get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _specialGameService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Delete get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (existData.IsActive == true)
            {
                return BadRequest("You cannot delete an active banner!");
            }
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", existData.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            await _specialGameService.DeleteAsync((int)id);
            _logger.LogInformation("SpecialGameBannerController/Delete called at {Time}", DateTime.UtcNow);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _specialGameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(new SpecialGameBannerUpdateVM()
            {
                Id=existData.Id,
                Name = existData.Name,
                IsActive = existData.IsActive,
                Description = existData.Description,
                Image = existData.Image,

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,SpecialGameBannerUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _specialGameService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/SetActiveBanner get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                request.Image = existData.Image;
                request.IsActive = existData.IsActive;
                return View(request);
            }

            if (request.NewImage != null)
            {
                string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", existData.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
                request.Image = fileName;
            }
            await _specialGameService.UpdateAsync((int)id, request);
            _logger.LogInformation("SpecialGameBannerController/Update called at {Time}", DateTime.UtcNow);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SetActiveBanner(int? id)
        {
            if (id == null)
            {
                _logger.LogError("SetActiveBanner/SetActiveBanner get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _specialGameService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("SetActiveBanner/SetActiveBanner get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            await _specialGameService.SetActiveBannerAsync((int)id);
            _logger.LogInformation("SpecialGameBannerController/SetActiveBanner called at {Time}", DateTime.UtcNow);
            return Ok();
        }
    }
}
