using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Discount;
using Service.ViewModels.Game;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IDiscountService _discountService;
        private readonly IGameImageService _gameImageService;
        private readonly ILogger<GameController> _logger;
        public GameController(IGameService gameService,
                              ICategoryService categoryService,
                              IWebHostEnvironment env,
                              IDiscountService discountService,
                              IGameImageService gameImageService,
                              ILogger<GameController> logger)
        {
            _gameService = gameService;
            _categoryService = categoryService;
            _env = env;
            _discountService = discountService;
            _gameImageService = gameImageService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _gameService.GetAllAsync();
            _logger.LogInformation("Game/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            _logger.LogInformation("Game/Detail called at {Time}", DateTime.UtcNow);
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


            await _gameService.CreateAsync(request);
            _logger.LogInformation("Game/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            foreach (var item in existData .GameImages)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Game-Images", item.Name);
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            await _gameService.DeleteAsync((int)id);
            _logger.LogInformation("Game/Delete called at {Time}", DateTime.UtcNow);

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name,
            }).ToList();

            var discounts = await _discountService.GetAllAsync();
            ViewBag.Discounts = discounts.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Value.ToString(),
            }).ToList();

            return View(new GameUpdateVM
            {
                NewName = existData.Name,
                NewPrice = existData.Price,
                NewDescription = existData.Description,
                NewStockCount = existData.StockCount,
                Images = existData.GameImages.Select(img => new GameImageVM
                {
                    Id=img.Id,
                    IsMain = img.IsMain,
                    Name = img.Name
                }).ToList(),

                SelectedCategoryIds = existData.GameCategory.Select(c => c.Id).ToList(),
                SelectedDiscountIds = existData.GameDiscounts.Select(d => d.Id).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,GameUpdateVM request)
        {
            if (id == null) return BadRequest();

            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name,
            }).ToList();

            var discounts = await _discountService.GetAllAsync();
            ViewBag.Discounts = discounts.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Value.ToString(),
            }).ToList();

            if (!ModelState.IsValid)
            {
                request.Images = existData.GameImages.Select(img => new GameImageVM
                {
                    Id = img.Id,
                    IsMain = img.IsMain,
                    Name = img.Name
                }).ToList();

                return View(request);
            }

            if (request.UploadImages != null)
            {
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

                request.Images = images;
            }
           
            await _gameService.UpdateAsync((int)id,request);
            _logger.LogInformation("Game/Update called at {Time}", DateTime.UtcNow);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGameImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _gameImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();

            if (existImage.IsMain == true)
            {
                ModelState.AddModelError("name", "The main image cannot be deleted!");
                return View(existImage);
            }
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Game-Images", existImage.Name);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _gameImageService.DeleteAsync((int)id);
            _logger.LogInformation("Game/DeleteGameImage called at {Time}", DateTime.UtcNow);

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> SetMainImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _gameImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();
            await _gameImageService.SetMainImage((int)id);
            _logger.LogInformation("Game/SetMainImage called at {Time}", DateTime.UtcNow);

            return Ok();
        }
    }
}
