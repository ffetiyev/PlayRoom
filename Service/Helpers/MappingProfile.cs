﻿using AutoMapper;
using Domain.Models;
using Domain.Models.Accessory;
using Domain.Models.Console;
using Domain.Models.Game;
using Domain.Models.News;
using Service.ViewModels.Accessory;
using Service.ViewModels.Category;
using Service.ViewModels.Company;
using Service.ViewModels.Console;
using Service.ViewModels.Discount;
using Service.ViewModels.Game;
using Service.ViewModels.News;
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

            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryCreateVM, Category>();

            CreateMap<GameImage, GameImageVM>();
            CreateMap<ConsoleImage, ConsoleImageVM>();
            CreateMap<AccessoryImage, AccessoryImageVM>();

            CreateMap<News, NewsVM>();
            //CreateMap<GameCategory, CategoryVM>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));

            //CreateMap<Game, GameVM>()
            //    .ForMember(dest => dest.GameImages, opt => opt.MapFrom(src => src.GameImages))
            //    .ForMember(dest => dest.GameDiscounts, opt => opt.MapFrom(src => src.GameDiscounts))
            //    .ForMember(dest => dest.GameCategory, opt => opt.MapFrom(src => src.GameCategories));


        }
    }
}
