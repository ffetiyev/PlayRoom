using Microsoft.AspNetCore.Mvc;
using Service.Service.Interfaces;
using Service.ViewModels;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IConsoleService _consoleService;
        private readonly IAccessoryService _accessoryService;
        public SearchController(IGameService gameService, 
                                IConsoleService consoleService, 
                                IAccessoryService accessoryService)
        {
            _gameService = gameService;
            _consoleService = consoleService;
            _accessoryService = accessoryService;
        }
        public async Task<IActionResult> Index(string? searchText)
        {
            if (searchText == null) return NotFound();
            var games = await _gameService.GetAllAsync();
            var consoles = await _consoleService.GetAllAsync();
            var accessories = await _accessoryService.GetAllAsync();

            var keyword = searchText.ToLower().Trim();

            var filteredGames = games.Where(m => m.Name.ToLower().Trim().Contains(keyword)).ToList();
            var filteredConsoles = consoles.Where(m => m.Name.ToLower().Trim().Contains(keyword)).ToList();
            var filteredAccessories = accessories.Where(m => m.Name.ToLower().Trim().Contains(keyword)).ToList();


            SearchVM model = new SearchVM()
            {
                Accessories= filteredAccessories,
                Consoles = filteredConsoles,
                Games= filteredGames
            };
            return View(model);
        }

    }
}
