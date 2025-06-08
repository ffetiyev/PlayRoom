using Microsoft.AspNetCore.Http;
using Service.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Game
{
    public class GameUpdateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string NewName { get; set; }

        [Required(ErrorMessage = "Price cannot be empty!")]
        public decimal? NewPrice { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string NewDescription { get; set; }

        [Required(ErrorMessage = "Stock count cannot be empty")]
        public int? NewStockCount { get; set; }
        public IEnumerable<IFormFile>? UploadImages { get; set; }
        public IEnumerable<GameImageVM>? Images { get; set; }
        public List<int>? CategoryIds { get; set; } 
        public List<int>? Discounts { get; set; }
        public List<int>? SelectedCategoryIds { get; set; } = new();
        public List<int>? SelectedDiscountIds { get; set; } = new();


    }
}
