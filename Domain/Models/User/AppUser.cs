using Microsoft.AspNetCore.Identity;

namespace Domain.Models.User
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
