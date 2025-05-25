using Domain.Commons;

namespace Domain.Models
{
    public class WelcomeBanner :BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
