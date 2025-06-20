using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Console;
using Service.ViewModels.HomeShortcut;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeShortcutController : Controller
    {
        private readonly IHomeShortcutService _homeShortcutService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HomeShortcutController> _logger;
        public HomeShortcutController(IHomeShortcutService homeShortcutService,
                                       IWebHostEnvironment env,
                                       ILogger<HomeShortcutController> logger)
        {
            _homeShortcutService = homeShortcutService;
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _homeShortcutService.GetAllAsync();
            _logger.LogInformation("HomeShortcut/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _homeShortcutService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            return View(new HomeShortcutUpdateVM { Title=existData.Title,Description=existData.Description,Image=existData.Image});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,HomeShortcutUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _homeShortcutService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Discount/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                request.Image = existData.Image;
                return View(request);
            }

            if (request.UploadImage != null)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "HomeShotcuts", existData.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                string newFileName = Guid.NewGuid().ToString() + "" + request.UploadImage.FileName;
                string newFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "HomeShotcuts", newFileName);
                using(FileStream stream = new(newFilePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                request.Image = newFileName;
            }
            await _homeShortcutService.UpdateAsync((int)id, request);
            _logger.LogInformation("HomeShortcut/Index called at {Time}", DateTime.UtcNow);


            return RedirectToAction(nameof(Index));
        }
    }
}
