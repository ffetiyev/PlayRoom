using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id=1)
        {
            if (id == null) return BadRequest();
            if (id < 1) id = 1;
            var data = await _gameService.GetAllPaginated((int)id);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPaginatedGames(int page = 1)
        {
            if (page < 1) page = 1;

            var data = await _gameService.GetAllPaginated(page);
            return Ok(data);
        }
    }
}
