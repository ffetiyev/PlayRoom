using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
{
    public class CategoryUpdateVM
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }
    }
}
