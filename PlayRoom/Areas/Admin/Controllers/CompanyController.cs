using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Company;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ICompanyService companyService, 
                                 IWebHostEnvironment env,
                                 ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _env = env;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _companyService.GetAllAsync();
            _logger.LogInformation("Company/Index called at {Time}", DateTime.UtcNow);
            return View(datas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateVM request)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("Company/Create get error at {Time}", DateTime.UtcNow);
                return View(request);
            }

            if (request.UploadImage != null)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "companies",fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }
                request.Image = fileName;
            }
            await _companyService.CreateAsync(request);
            _logger.LogInformation("Company/Create called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var existData = await _companyService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            return View(new CompanyUpdateVM()
            {
                Image = existData.Image,
                Name = existData.Name,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CompanyUpdateVM request)
        {
            if (id == null)
            {
                _logger.LogError("Company/Update get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _companyService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Company/Update get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Company/Update get error at {Time}", DateTime.UtcNow);
                request.Image = existData.Image;

                return View(request);
            }

            if (request.UploadImage != null)
            {
                string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "companies", existData.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "companies", fileName);
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                request.Image = fileName;
            }
            await _companyService.UpdateAsync((int)id, request);
            _logger.LogInformation("Company/Update called at {Time}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Company/Delete get error at {Time}", DateTime.UtcNow);
                return BadRequest();
            }
            var existData = await _companyService.GetByIdAsync((int)id);
            if (existData == null)
            {
                _logger.LogError("Company/Delete get error at {Time}", DateTime.UtcNow);
                return NotFound();
            }

            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "companies", existData.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _companyService.DeleteAsync((int) id);
            _logger.LogInformation("Company/Delete called at {Time}", DateTime.UtcNow);
            return Ok();
        }
    }
}
