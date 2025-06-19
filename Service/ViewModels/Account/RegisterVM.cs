using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required,DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifrə ən az 6 simvol olmalıdır.")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare("Password", ErrorMessage = "Təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }
}
