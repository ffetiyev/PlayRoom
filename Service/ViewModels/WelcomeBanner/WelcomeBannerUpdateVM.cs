using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.WelcomeBanner
{
    public class WelcomeBannerUpdateVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
