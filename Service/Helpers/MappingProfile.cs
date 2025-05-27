using AutoMapper;
using Domain.Models;
using Service.ViewModels;

namespace Service.Helpers
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            CreateMap<WelcomeBanner, WelcomeBannerVM>();

            CreateMap<SpecialGameBanner, SpecialGameBannerVM>();
        }
    }
}
