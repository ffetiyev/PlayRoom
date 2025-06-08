using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessoryController : Controller
    {
        private readonly IAccessoryService _accessoryService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IDiscountService _discountService;
        private readonly IAccessoryImageService _accessoryImageService;
        public AccessoryController(IAccessoryService accessoryService, 
                                   ICategoryService categoryService,
                                   IWebHostEnvironment env,
                                   IDiscountService discountService,
                                   IAccessoryImageService accessoryImageService)
        {
            _accessoryService = accessoryService;
            _categoryService = categoryService;
            _env = env;
            _discountService = discountService;
            _accessoryImageService = accessoryImageService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _accessoryService.GetAllAsync();
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccessoryCreateVM request)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            if (!ModelState.IsValid) return View(request);

            foreach (var item in request.UploadImages)
            {
                if (!item.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("UploadImages", "File type should be only image!");
                    return View(request);
                }
                if(item.Length/1024 > 2000)
                {
                    ModelState.AddModelError("UploadImages", "Images size should be less than 2 mb!");
                    return View(request);
                }
            }
            List<AccessoryImageVM> images = new();
            foreach (var image in request.UploadImages)
            {
                string fileName = Guid.NewGuid() + "_" + image.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Accessories", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }


                images.Add(new AccessoryImageVM
                {
                    Name = fileName,
                    IsMain = false
                });
            }
            if (images.Any())
                images.First().IsMain = true;

            request.Images = images;


            await _accessoryService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _accessoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _accessoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            foreach (var item in existData.Images)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Accessories", item.Name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            await _accessoryService.DeleteAsync((int)id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();


            var existData = await _accessoryService.GetByIdAsync((int)id);
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

            return View(new AccessoryUpdateVM
            {
                NewName = existData.Name,
                NewPrice = existData.Price,
                NewDescription = existData.Description,
                NewStockCount = existData.StockCount,
                Images = existData.Images.Select(img => new AccessoryImageVM
                {
                    Id = img.Id,
                    IsMain = img.IsMain,
                    Name = img.Name
                }).ToList(),

                SelectedCategoryIds = existData.Category.Select(c => c.Id).ToList(),
                SelectedDiscountIds = existData.Discounts.Select(d => d.Id).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AccessoryUpdateVM request)
        {
            if (id == null) return BadRequest();

            var existData = await _accessoryService.GetByIdAsync((int)id);
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
                request.Images = existData.Images.Select(img => new AccessoryImageVM
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

                List<AccessoryImageVM> images = new();
                foreach (var image in request.UploadImages)
                {
                    string fileName = Guid.NewGuid() + "_" + image.FileName;
                    string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Accessories", fileName);

                    using (FileStream stream = new(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }


                    images.Add(new AccessoryImageVM
                    {
                        Name = fileName,
                        IsMain = false
                    });
                }

                request.Images = images;
            }

            await _accessoryService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGameImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _accessoryImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();

            if (existImage.IsMain == true)
            {
                ModelState.AddModelError("name", "The main image cannot be deleted!");
                return View(existImage);
            }
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "Accessories", existImage.Name);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _accessoryImageService.DeleteAsync((int)id);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> SetMainImage(int? id)
        {
            if (id == null) return BadRequest();
            var existImage = await _accessoryImageService.GetByIdAsync((int)id);
            if (existImage == null) return NotFound();
            await _accessoryImageService.SetMainImage((int)id);
            return Ok();
        }
    }
}
