using Service.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Game
{
    public class GameUpdateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price cannot be empty!")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Stock count cannot be empty")]
        public int StockCount { get; set; }
        public List<GameImageVM>? UploadImages { get; set; }
        public List<GameImageVM>? Images { get; set; }
        public List<GameDiscountVM>? GameDiscounts { get; set; }
        [Required(ErrorMessage = "Category cannot be empty")]
        public List<int> CategorieIds { get; set; }
    }
}
