using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Basket;
using Service.ViewModels.Console;
using Service.ViewModels.Favorites;
using Service.ViewModels.Game;
using System.Threading.Tasks;

namespace PlayRoom.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGameService _gameService;
        private readonly IConsoleService _consoleService;
        private readonly IAccessoryService _accessoryService;
        private readonly IPromocodeService _promocodeService;
        public CartController(IHttpContextAccessor contextAccessor,
                              IGameService gameService,
                              IConsoleService consoleService,
                              IAccessoryService accessoryService,
                              IPromocodeService promocodeService)
        {
            _contextAccessor = contextAccessor;
            _gameService = gameService;
            _consoleService = consoleService;
            _accessoryService = accessoryService;
            _promocodeService = promocodeService;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas = new List<BasketVM>();
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            ViewBag.Favorites = favoriteDatas
                .Select(f => (f.ProductId, f.ProductType))
                .ToList();

            Dictionary<GameVM, int> games = new Dictionary<GameVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "game"))
            {
                var game = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(game, item.ProductCount);
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


            return View(new BasketDetailVM { Accessories = accessories, Consoles = consoles, Games = games, Total = totalPrice, TotalDiscountless = totalDiscountless });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id,string type)
        {
            List<BasketVM> basketDatas = new List<BasketVM>();
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            
            var existBasketDatas = basketDatas.Where(m=>m.ProductType == type && m.ProductCount==id);
            basketDatas.RemoveAll(m => m.ProductType == type && m.ProductId == id);
            _contextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            Dictionary<GameVM, int> games = new Dictionary<GameVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "game"))
            {
                var game = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(game, item.ProductCount);
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

            int basketCount = basketDatas.Sum(m => m.ProductCount);
            //Price without discount
            decimal totalPrice = gamesPrice + consolesPrice + accessoriesPrice;

            decimal gamesOwnPrice = games.Sum(m => m.Key.Price * m.Value);
            decimal consolesOwnPrice = consoles.Sum(m => m.Key.Price * m.Value);
            decimal accessoriesOwnPrice = accessories.Sum(m => m.Key.Price * m.Value);

            decimal totalDiscountless = gamesOwnPrice + consolesOwnPrice + accessoriesOwnPrice;
            decimal totalDiscount = totalDiscountless - totalPrice;

            decimal[] data = { basketCount, totalPrice,totalDiscountless,totalDiscount };
            return Ok(data);
        }

        public async Task<IActionResult> IncreaseProductCount(int id, string type)
        {
            List<BasketVM> basketDatas = new List<BasketVM>();
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            var existBasketDatas = basketDatas.FirstOrDefault(m => m.ProductType == type && m.ProductId == id);
            if (existBasketDatas != null)
            {
                existBasketDatas.ProductCount++;
            }

            _contextAccessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));

            Dictionary<GameVM, int> games = new Dictionary<GameVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "game"))
            {
                var game = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(game, item.ProductCount);
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

            int basketCount = basketDatas.Sum(m => m.ProductCount);
            //Price without discount
            decimal totalPrice = gamesPrice + consolesPrice + accessoriesPrice;

            decimal gamesOwnPrice = games.Sum(m => m.Key.Price * m.Value);
            decimal consolesOwnPrice = consoles.Sum(m => m.Key.Price * m.Value);
            decimal accessoriesOwnPrice = accessories.Sum(m => m.Key.Price * m.Value);

            decimal totalDiscountless = gamesOwnPrice + consolesOwnPrice + accessoriesOwnPrice;
            decimal totalDiscount = totalDiscountless - totalPrice;

            decimal[] data = { basketCount, totalPrice, totalDiscountless, totalDiscount };
            return Ok(data);

        }
        public async Task<IActionResult> DecreaseProductCount(int id, string type)
        {
            List<BasketVM> basketDatas = new List<BasketVM>();
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            var existBasketDatas = basketDatas.FirstOrDefault(m => m.ProductType == type && m.ProductId == id);
            if (existBasketDatas != null)
            {
                existBasketDatas.ProductCount--;
            }

            _contextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            Dictionary<GameVM, int> games = new Dictionary<GameVM, int>();

            foreach (var item in basketDatas.Where(m => m.ProductType == "game"))
            {
                var game = await _gameService.GetByIdAsync(item.ProductId);
                games.Add(game, item.ProductCount);
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

            int basketCount = basketDatas.Sum(m => m.ProductCount);
            //Price without discount
            decimal totalPrice = gamesPrice + consolesPrice + accessoriesPrice;

            decimal gamesOwnPrice = games.Sum(m => m.Key.Price * m.Value);
            decimal consolesOwnPrice = consoles.Sum(m => m.Key.Price * m.Value);
            decimal accessoriesOwnPrice = accessories.Sum(m => m.Key.Price * m.Value);

            decimal totalDiscountless = gamesOwnPrice + consolesOwnPrice + accessoriesOwnPrice;
            decimal totalDiscount = totalDiscountless - totalPrice;

            decimal[] data = { basketCount, totalPrice, totalDiscountless, totalDiscount };
            return Ok(data);

        }

        [HttpPost]
        public async Task<IActionResult> AddPromocode(string promocode)
        {
            if (string.IsNullOrWhiteSpace(promocode))
                return Ok("There is no such promocode!");

            var promocodes = await _promocodeService.GetAllAsync();
            var existPromocode = promocodes.FirstOrDefault(m => m.Name == promocode);

            if (existPromocode == null)
                return Ok("There is no such promocode!");

            return Ok(existPromocode.Value.ToString());
        }


    }
}
