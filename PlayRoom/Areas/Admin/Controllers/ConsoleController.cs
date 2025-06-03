using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service;
using Service.Service.Interfaces;
using Service.ViewModels;
using Service.ViewModels.Console;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ConsoleController : Controller
    {
        private readonly IConsoleService _consoleService;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;
        public ConsoleController(IConsoleService consoleService, 
                                 IWebHostEnvironment env,
                                 ICategoryService categoryService)
        {
            _consoleService = consoleService;
            _env = env;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _consoleService.GetAllAsync();
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name,
            }).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsoleCreateVM request)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name,
            }).ToList();

            if (!ModelState.IsValid) return View(request);

            foreach(var image in request.UploadImages)
            {
                if (!image.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("UploadImages", "File type must be only image!");
                    return View(request);
                }
                if (image.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("UploadImages", "Images size should be less than 2 mb!");
                    return View(request);
                }
            }
            List<GameImageVM> images = new();
            foreach (var image in request.UploadImages)
            {
                string fileName = Guid.NewGuid() + "_" + image.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Console-Images", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }


                images.Add(new GameImageVM
                {
                    Name = fileName,
                    IsMain = false
                });
            }
            if (images.Any())
                images.First().IsMain = true;

            request.Images = images;

            await _consoleService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existConsole= await _consoleService.GetByIdAsync((int)id);
            if (existConsole == null) return NotFound();
            return View(existConsole);
        }
    }
}
