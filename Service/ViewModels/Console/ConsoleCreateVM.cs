using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Console
{
    public class ConsoleCreateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price cannot be empty!")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Memory cannot be empty")]
        public int? Memory { get; set; }

        [Required(ErrorMessage = "Stock count cannot be empty")]
        public int? StockCount { get; set; }

        [Required(ErrorMessage = "Image cannot be empty")]
        public IEnumerable<IFormFile> UploadImages { get; set; }
        public IEnumerable<GameImageVM>? Images { get; set; }

        [MinLength(1, ErrorMessage = "At least one category must be selected.")]
        public List<int>? CategoryIds { get; set; } = new();
    }
}
