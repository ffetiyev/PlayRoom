using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Accessory
{
    public class AccessoryCreateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price cannot be empty!")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Stock count cannot be empty")]
        public int? StockCount { get; set; }

        [Required(ErrorMessage = "Image cannot be empty")]
        public IEnumerable<IFormFile> UploadImages { get; set; }

        public IEnumerable<AccessoryImageVM>? Images { get; set; }
        [MinLength(1, ErrorMessage = "At least one category must be selected.")]
        public List<int>? CategoryIds { get; set; } = new();
    }
}
