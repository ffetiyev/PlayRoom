using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Console;
using Service.ViewModels.HomeShortcut;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeShortcutController : Controller
    {
        private readonly IHomeShortcutService _homeShortcutService;
        private readonly IWebHostEnvironment _env;
        public HomeShortcutController(IHomeShortcutService homeShortcutService,
                                       IWebHostEnvironment env)
        {
            _homeShortcutService = homeShortcutService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _homeShortcutService.GetAllAsync();
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _homeShortcutService.GetByIdAsync((int)id);
            if (existData == null) return NotFound(); 

            return View(new HomeShortcutUpdateVM { Title=existData.Title,Description=existData.Description,Image=existData.Image});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,HomeShortcutUpdateVM request)
        {
            if (id == null) return BadRequest();
            var existData = await _homeShortcutService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

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

            return RedirectToAction(nameof(Index));
        }
    }
}
