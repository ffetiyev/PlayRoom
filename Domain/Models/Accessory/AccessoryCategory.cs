using Domain.Commons;

namespace Domain.Models.Accessory
{
   public class AccessoryCategory : BaseEntity
    {
        public int AccessoryId { get; set; }
        public Accessory Accessory { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
