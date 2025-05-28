using AutoMapper;
using Domain.Models;
using Service.ViewModels;
using Service.ViewModels.Discount;
using Service.ViewModels.WelcomeBanner;

namespace Service.Helpers
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            CreateMap<WelcomeBanner, WelcomeBannerVM>();

            CreateMap<SpecialGameBanner, SpecialGameBannerVM>();

            CreateMap<Discount, DiscountVM>();
            CreateMap<DiscountCreateVM, Discount>();
        }
    }
}
