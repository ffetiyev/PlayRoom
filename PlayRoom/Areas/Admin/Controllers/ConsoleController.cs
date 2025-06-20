using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service;
using Service.Service.Interfaces;
using Service.ViewModels;
using Service.ViewModels.Accessory;
using Service.ViewModels.Console;
using Service.ViewModels.Game;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ConsoleController : Controller
    {
        private readonly IConsoleService _consoleService;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;
        private readonly IDiscountService _discountService;
        private readonly IConsoleImageService _consoleImageService;
        private readonly ILogger<ConsoleController> _logger;
        public ConsoleController(IConsoleService consoleService, 
                                 IWebHostEnvironment env,
                                 ICategoryService categoryService,
                                 IDiscountService discountService,
                                 IConsoleImageService consoleImageService,
                                 ILogger<ConsoleController> logger)
        {
            _consoleService = consoleService;
            _env = env;
            _categoryService = categoryService;
            _discountService = discountService;
            _consoleImageService = consoleImageService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _consoleService.GetAllAsync();
            _logger.LogInformation("Console/Index called at {Time}", DateTime.UtcNow);
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

            if (!ModelState.IsValid)
            {
                _logger.LogError("Console/Create get error at {Time}", DateTime.UtcNow);
                return View(request);
            }

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
            List<ConsoleImageVM> images = new();
            foreach (var image in request.UploadImages)
            {
                string fileName = Guid.NewGuid() + "_" + image.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Console-Images", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }


                images.Add(new ConsoleImageVM
                {
                    Name = fileName,
                    IsMain = false
                });
            }
            if (images.Any())
                images.First().IsMain = true;

            request.Images = images;

            await _consoleService.CreateAsync(request);
            _logger.LogInformation("Console/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Console/Detail get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existConsole= await _consoleService.GetByIdAsync((int)id);
            if (existConsole == null)
            {
                _logger.LogError("Console/Detail get error at {Time}", DateTime.UtcNow);
                  return NotFound();
            }
            _logger.LogInformation("Console/Detail called at {Time}", DateTime.UtcNow);
            return View(existConsole);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Console/Delete get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _consoleService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Console/Delete get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            await _consoleService.DeleteAsync((int)id);

            foreach (var item in existData.Images)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Console-Images", item.Name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _logger.LogInformation("Console/Delete called at {Time}", DateTime.UtcNow);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var existData = await _consoleService.GetByIdAsync((int)id);
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

            return View(new ConsoleUpdateVM
            {
                NewName = existData.Name,
                NewPrice = existData.Price,
                NewDescription = existData.Description,
                NewStockCount = existData.StockCount,
                NewMemory = existData.Memory,
                Images = existData.Images.Select(img => new ConsoleImageVM
                {
                    Id = img.Id,
                    IsMain = img.IsMain,
                    Name = img.Name
                }).ToList(),

                SelectedCategoryIds = existData.Categories.Select(c => c.Id).ToList(),
                SelectedDiscountIds = existData.Discounts.Select(d => d.Id).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ConsoleUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Console/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }

            var existData = await _consoleService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Console/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

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
                request.Images = existData.Images.Select(img => new ConsoleImageVM
                {
                    Id = img.Id,
                    IsMain = img.IsMain,
                    Name = img.Name
                }).ToList();
                _logger.LogError("Console/Update get error at {Time}", DateTime.UtcNow);

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

                List<ConsoleImageVM> images = new();
                foreach (var image in request.UploadImages)
                {
                    string fileName = Guid.NewGuid() + "_" + image.FileName;
                    string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Console-Images", fileName);

                    using (FileStream stream = new(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }


                    images.Add(new ConsoleImageVM
                    {
                        Name = fileName,
                        IsMain = false
                    });
                }

                request.Images = images;
            }

            await _consoleService.UpdateAsync((int)id, request);
            _logger.LogInformation("Console/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConsoleImage(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Console/SetMainImage get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existImage = await _consoleImageService.GetByIdAsync((int)id);
            if (existImage == null)
            {
                _logger.LogError("Console/SetMainImage get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            if (existImage.IsMain == true)
            {
                ModelState.AddModelError("name", "The main image cannot be deleted!");
                return View(existImage);
            }
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Game-Images", existImage.Name);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _consoleImageService.DeleteAsync((int)id);
            _logger.LogInformation("Console/DeleteConsoleImage called at {Time}", DateTime.UtcNow);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetMainImage(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Console/SetMainImage get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existImage = await _consoleImageService.GetByIdAsync((int)id);
            if (existImage == null)
            {
                _logger.LogError("Console/SetMainImage get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            await _consoleImageService.SetMainImage((int)id);
            _logger.LogInformation("Console/SetMainImage called at {Time}", DateTime.UtcNow);

            return Ok();
        }

    }
}
