using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Promocode
{
    public class PromocodeCreateVM
    {
        [Required(ErrorMessage ="Name cannot be empty")]
        [MinLength(5,ErrorMessage ="Minimum length is 5")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Value cannot be empty")]
        public int Value { get; set; }
    }
}
