using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service.Interfaces;
using Service.ViewModels;
using Service.ViewModels.Game;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public GameController(IGameService gameService,
                              ICategoryService categoryService,
                              IWebHostEnvironment env)
        {
            _gameService = gameService;
            _categoryService = categoryService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _gameService.GetAllAsync();
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(m=>new SelectListItem
            {
                Value = m.Id.ToString(),
                Text=m.Name,
            }).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameCreateVM request)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            if (!ModelState.IsValid) return View(request);
            foreach (var image in request.UploadImages)
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
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Game-Images", fileName);

                using FileStream stream = new(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                images.Add(new GameImageVM
                {
                    Name = fileName, 
                    IsMain = false
                });
            }

            if (images.Any())
                images.First().IsMain = true;

            request.Images = images;


            await _gameService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
