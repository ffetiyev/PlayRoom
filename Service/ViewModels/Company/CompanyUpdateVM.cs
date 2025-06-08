using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Company
{
    public class CompanyUpdateVM
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? UploadImage { get; set; }
    }
}
