using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;


namespace PlayRoom.Controllers
{
    public class ConsoleDetailController : Controller
    {
        private readonly IConsoleService _consoleService;
        public ConsoleDetailController(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _consoleService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
    }
}
