using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SpecialGameBanner
{
    public class SpecialGameBannerCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public IFormFile NewImage { get; set; }
    }
    //public class SpecialGameBannerCreateVMValidator : AbstractValidator<SpecialGameBannerCreateVM>
    //{
    //    public SpecialGameBannerCreateVMValidator()
    //    {
    //        RuleFor(x => x.Name)
    //            .NotNull().WithMessage("Name cannot be empty!");
    //    }
    //}
}
