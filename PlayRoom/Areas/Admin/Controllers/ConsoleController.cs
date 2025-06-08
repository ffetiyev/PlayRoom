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
    public class ConsoleController : Controller
    {
        private readonly IConsoleService _consoleService;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;
        private readonly IDiscountService _discountService;
        private readonly IConsoleImageService _consoleImageService;
        public ConsoleController(IConsoleService consoleService, 
                                 IWebHostEnvironment env,
                                 ICategoryService categoryService,
                                 IDiscountService discountService,
                                 IConsoleImageService consoleImageService)
        {
            _consoleService = consoleService;
            _env = env;
            _categoryService = categoryService;
            _discountService = discountService;
            _consoleImageService = consoleImageService;
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
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _consoleService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            await _consoleService.DeleteAsync((int)id);

            foreach (var item in existData.Images)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Console-Images", item.Name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

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

            if (!ModelState.IsValid)
            {
                request.Images = existData.Images.Select(img => new ConsoleImageVM
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

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGameImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _consoleImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();

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
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetMainImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _consoleImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();
            await _consoleImageService.SetMainImage((int)id);
            return Ok();
        }

    }
}
