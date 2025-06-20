using Domain.Commons;

namespace Domain.Models.Console
{
    public class ConsoleDiscount : BaseEntity
    {
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int ConsoleId { get; set; }
        public Console Console { get; set; }
    }
}
