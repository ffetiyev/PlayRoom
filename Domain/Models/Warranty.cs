using Domain.Commons;

namespace Domain.Models
{
    public class Warranty : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
