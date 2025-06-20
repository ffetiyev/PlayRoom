using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Category;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, 
                                  ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _categoryService.GetAllAsync();
            _logger.LogInformation("Category/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("Category/Create get error at {Time}", DateTime.UtcNow);
                return View(request);
            }
            await _categoryService.CreateAsync(request);
            _logger.LogInformation("Category/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _categoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            return View(new CategoryUpdateVM() { Name = existData.Name});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,CategoryUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Category/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _categoryService.GetByIdAsync((int)id);
            if (existData == null) 
            {
                _logger.LogError("Category/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Category/Update get error at {Time}", DateTime.UtcNow);
                return View(request);
            }
            await _categoryService.UpdateAsync((int) id, request);
            _logger.LogInformation("Category/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Category/Delete get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _categoryService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Category/Delete get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            await _categoryService.DeleteAsync((int) id);
            return Ok();
        }
    }
}
