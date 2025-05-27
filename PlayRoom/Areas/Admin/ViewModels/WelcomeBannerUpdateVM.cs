namespace PlayRoom.Areas.Admin.ViewModels
{
    public class WelcomeBannerUpdateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
