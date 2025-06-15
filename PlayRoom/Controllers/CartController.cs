using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Basket;
using Service.ViewModels.Console;
using Service.ViewModels.Game;

namespace PlayRoom.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGameService _gameService;
        private readonly IConsoleService _consoleService;
        private readonly IAccessoryService _accessoryService;
        public CartController(IHttpContextAccessor contextAccessor,
                              IGameService gameService,
                              IConsoleService consoleService,
                              IAccessoryService accessoryService)
        {
            _contextAccessor = contextAccessor;
            _gameService = gameService;
            _consoleService = consoleService;
            _accessoryService = accessoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas= new List<BasketVM>();
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            Dictionary<GameVM,int> games = new Dictionary<GameVM,int>();

            foreach(var item in basketDatas.Where(m=>m.ProductType=="game"))
            {
                var game = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(game,item.ProductCount);
            }

            Dictionary<ConsoleVM, int> consoles = new Dictionary<ConsoleVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "console"))
            {
                var console = await _consoleService.GetByIdAsync(item.ProductId);
                consoles.Add(console, item.ProductCount);
            }

            Dictionary<AccessoryVM, int> accessories = new Dictionary<AccessoryVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "accessory"))
            {
                var accessory = await _accessoryService.GetByIdAsync(item.ProductId);
                accessories.Add(accessory, item.ProductCount);
            }
            decimal gamesPrice = games.Sum(m =>
            {
                decimal finalPrice = m.Key.Price;
                if (m.Key.GameDiscounts != null && m.Key.GameDiscounts.Any())
                {
                    foreach (var discount in m.Key.GameDiscounts)
                    {
                        finalPrice *= (1 - discount.Value / 100);
                    }
                }
                return finalPrice * m.Value;
            });

            decimal consolesPrice = consoles.Sum(m =>
            {
                decimal finalPrice = m.Key.Price;
                if (m.Key.Discounts != null && m.Key.Discounts.Any())
                {
                    foreach (var discount in m.Key.Discounts)
                    {
                        finalPrice *= (1 - discount.Value / 100);
                    }
                }
                return finalPrice * m.Value;
            });

            decimal accessoriesPrice = accessories.Sum(m =>
            {
                decimal finalPrice = m.Key.Price;
                if (m.Key.Discounts != null && m.Key.Discounts.Any())
                {
                    foreach (var discount in m.Key.Discounts)
                    {
                        finalPrice *= (1 - discount.Value / 100);
                    }
                }
                return finalPrice * m.Value;
            });

            //Price without discount
            decimal totalPrice = gamesPrice + consolesPrice + accessoriesPrice;

            decimal gamesOwnPrice = games.Sum(m => m.Key.Price * m.Value);
            decimal consolesOwnPrice = consoles.Sum(m => m.Key.Price * m.Value);
            decimal accessoriesOwnPrice = accessories.Sum(m => m.Key.Price * m.Value);

            decimal totalDiscountless = gamesOwnPrice + consolesOwnPrice + accessoriesOwnPrice;


            return View(new BasketDetailVM { Accessories=accessories,Consoles=consoles,Games=games,Total=totalPrice,TotalDiscountless=totalDiscountless});
        }

    }
}
