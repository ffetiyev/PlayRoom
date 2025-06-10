using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.News
{
   public class NewsCreateVM
    {
        [Required(ErrorMessage ="TItle cannot be empty!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description cannot be empty!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Image cannot be empty!")]
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
        public string? VideoLink { get; set; }
    }
}
