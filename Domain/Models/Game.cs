using Domain.Commons;

namespace Domain.Models
{
    public class Game :BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<GameImage> GameImages { get; set; }
        public List<GameDiscount> GameDiscounts { get; set; }
        public ICollection<GameCategory> GameCategories { get; set; }
    }
}
