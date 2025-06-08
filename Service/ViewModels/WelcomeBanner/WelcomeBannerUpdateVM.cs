using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.WelcomeBanner
{
    public class WelcomeBannerUpdateVM
    {
        [Required(ErrorMessage ="Title cannot be empty!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description cannot be empty!")]
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
