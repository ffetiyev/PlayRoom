using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.HomeShortcut
{
    public class HomeShortcutUpdateVM
    {
        [Required(ErrorMessage = "Title cannor be empty!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description cannor be empty!")]
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? UploadImage { get; set; }
    }
}
