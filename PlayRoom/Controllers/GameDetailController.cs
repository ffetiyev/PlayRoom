using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;

namespace PlayRoom.Controllers
{
    public class GameDetailController : Controller
    {
        private readonly IGameService _gameService;
        public GameDetailController(IGameService gameService)
        {
            _gameService = gameService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();
            var existData = await _gameService.GetByIdAsync((int)id);
            if (existData == null) return NotFound();
            return View(existData);
        }
    }
}
