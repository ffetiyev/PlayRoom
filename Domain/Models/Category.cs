using Domain.Commons;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<GameCategory> GameCategories { get; set; }
    }
}
