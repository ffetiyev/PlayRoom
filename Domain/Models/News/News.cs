using Domain.Commons;

namespace Domain.Models.News
{
    public class News : BaseEntity
    {
        public  string Title { get; set; }
        public  string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
