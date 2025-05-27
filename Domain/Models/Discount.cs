using Domain.Commons;

namespace Domain.Models
{
    public class Discount : BaseEntity
    {
        public decimal Value { get; set; }
        public List<GameDiscount> GameDiscounts { get; set; }
    }
}
