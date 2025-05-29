using FluentValidation;
using Service.ViewModels.Discount;

namespace Service.ViewModels.SpecialGameBanner
{
    public class SpecialGameBannerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
