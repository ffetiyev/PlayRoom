using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Discount
{
    public class DiscountUpdateVM
    {
        [Required(ErrorMessage ="Value cannot be empty!")]
        public int Value { get; set; }
    }
}
