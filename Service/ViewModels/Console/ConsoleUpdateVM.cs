using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Console
{
    public class ConsoleUpdateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string NewName { get; set; }

        [Required(ErrorMessage = "Price cannot be empty!")]
        public decimal? NewPrice { get; set; }

        [Required(ErrorMessage = "Description cannot be empty!")]
        public string NewDescription { get; set; }
        [Required(ErrorMessage = "Memory cannot be empty!")]
        public int? NewMemory { get; set; }

        [Required(ErrorMessage = "Stock count cannot be empty!")]
        public int? NewStockCount { get; set; }

        [Required(ErrorMessage = "Image cannot be empty!")]
        public IEnumerable<IFormFile> UploadImages { get; set; }
        public IEnumerable<ConsoleImageVM>? Images { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> Discounts { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new();
        public List<int> SelectedDiscountIds { get; set; } = new();
    }
}
