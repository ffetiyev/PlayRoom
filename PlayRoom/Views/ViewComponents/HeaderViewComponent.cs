using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.ViewModels;
using Service.ViewModels.Basket;

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
            int totalProducts = 0;
            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            totalProducts = basketDatas.Sum(m => m.ProductCount);


            return View(new HeaderVM
            {

                BasketProductCount = totalProducts
            });
        }
    }

}
