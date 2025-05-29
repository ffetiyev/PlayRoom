using AutoMapper;
using Domain.Models;
using Service.ViewModels.Company;
using Service.ViewModels.Discount;
using Service.ViewModels.SpecialGameBanner;
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

            CreateMap<SpecialGameBannerCreateVM, SpecialGameBanner>();
            CreateMap<SpecialGameBanner, SpecialGameBannerVM>();

            CreateMap<Company, CompanyVM>();
        }
    }
}
