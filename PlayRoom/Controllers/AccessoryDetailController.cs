using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class AccessoryDetailController : Controller
    {
        private readonly IAccessoryService _accessoryService;
        public AccessoryDetailController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _accessoryService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
    }
}

