using Domain.Commons;

namespace Domain.Models.Accessory
{
    public class AccessoryDiscount : BaseEntity
    {
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int AccessoryId { get; set; }
        public Accessory Accessory { get; set; }
    }
}
