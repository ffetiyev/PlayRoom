using Domain.Commons;
using Domain.Models.Accessory;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<GameCategory> GameCategories { get; set; }
        public ICollection<ConsoleCategory> ConsoleCategories { get; set; }
        public ICollection<AccessoryCategory> AccessoryCategories { get; set; }
    }
}
