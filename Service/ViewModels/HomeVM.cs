

using Service.ViewModels.SpecialGameBanner;
using Service.ViewModels.WelcomeBanner;

namespace Service.ViewModels
{
    public class HomeVM
    {
        public WelcomeBannerVM WelcomeBanner { get; set; }
        public IEnumerable<GameVM> Games { get; set; }
        public IEnumerable<SpecialGameBannerVM> SpecialGameBanners { get; set; }
    }
}
