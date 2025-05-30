using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Category;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _categoryService.GetAllAsync();
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
            if(!ModelState.IsValid) return View(request);
            await _categoryService.CreateAsync(request);
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
            if (id == null) return BadRequest();
            var existData = await _categoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            if (!ModelState.IsValid) return View(request);
            await _categoryService.UpdateAsync((int) id, request);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _categoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            await _categoryService.DeleteAsync((int) id);
            return Ok();
        }
    }
}
