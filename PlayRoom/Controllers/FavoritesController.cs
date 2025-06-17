using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.ViewModels.Favorites;

namespace PlayRoom.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public FavoritesController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddToFavorites(int id,string type)
        {
            List<FavoritesVM> favoriteDatas = new();
            if (_contextAccessor.HttpContext.Request.Cookies["favorites"]!=null)
            {
                favoriteDatas=JsonConvert.DeserializeObject<List<FavoritesVM>>(_contextAccessor.HttpContext.Request.Cookies["favorites"]);
            }

            var favoriteData = favoriteDatas.FirstOrDefault(m=>m.ProductId==id && m.ProductType==type);
            if (favoriteData==null)
            {
                favoriteDatas.Add(new FavoritesVM() { ProductId=id,ProductType=type});
            }

            _contextAccessor.HttpContext.Response.Cookies.Append("favorites",JsonConvert.SerializeObject(favoriteDatas));

                return Ok(favoriteDatas.Count());
        }
    }
}
