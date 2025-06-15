using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class WarrantyController : Controller
    {
        private readonly IWarrantyService _warrantyService;
        public WarrantyController(IWarrantyService warrantyService)
        {
            _warrantyService = warrantyService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _warrantyService.GetAsync();
            return View(data);
        }
    }
}
