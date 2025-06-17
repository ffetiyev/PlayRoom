using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.ViewModels;
using Service.ViewModels.Basket;
using Service.ViewModels.Favorites;

namespace PlayRoom.Views.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HeaderViewComponent(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVM> basketDatas = new();
            int totalBasketProducts = 0;
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            totalBasketProducts = basketDatas.Sum(m => m.ProductCount);


            List<FavoritesVM> favoriteDatas = new();
            int totalFavoritesProducts = 0;
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"] != null)
            {
                favoriteDatas = JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            totalFavoritesProducts = favoriteDatas.Count();

            return View(new HeaderVM
            {
                FavoritesCount = totalFavoritesProducts,
                BasketProductCount = totalBasketProducts
            });
        }
    }

}
