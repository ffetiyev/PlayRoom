using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Company;
using System.Threading.Tasks;

namespace PlayRoom.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _env;
        public CompanyController(ICompanyService companyService, 
                                 IWebHostEnvironment env)
        {
            _companyService = companyService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _companyService.GetAllAsync();
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
            if(!ModelState.IsValid) return View(request);

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
            if (id == null) return BadRequest();
            var existData = await _companyService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            if (!ModelState.IsValid)
            {
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

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _companyService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();

            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "companies", existData.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _companyService.DeleteAsync((int) id);
            return Ok();
        }
    }
}
