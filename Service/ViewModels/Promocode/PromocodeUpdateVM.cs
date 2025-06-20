using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Promocode
{
    public class PromocodeUpdateVM
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Value cannot be empty")]
        public int Value { get; set; }
    }
}
