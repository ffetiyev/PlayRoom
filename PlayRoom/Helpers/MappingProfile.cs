using AutoMapper;
using Domain.Models;
using PlayRoom.ViewModels;

namespace PlayRoom.Helpers
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            CreateMap<WelcomeBanner, WelcomeBannerVM>();
        }
    }
}
