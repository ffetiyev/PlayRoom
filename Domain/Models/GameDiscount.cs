using Domain.Commons;

namespace Domain.Models
{
    public class GameDiscount :BaseEntity
    {
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
