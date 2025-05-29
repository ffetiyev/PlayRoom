using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Company
{
    public class CompanyCreateVM
    {
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        [Required]
        public IFormFile UploadImage { get; set; }
    }
}
