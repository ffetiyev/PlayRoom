using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SpecialGameBanner
{
    public class SpecialGameBannerUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public IFormFile NewImage { get; set; }
    }
}
