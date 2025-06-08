using Domain.Commons;

namespace Domain.Models.Accessory
{
    public class Accessory : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<AccessoryImage> AccessoryImage { get; set; }
        public List<AccessoryDiscount> AccessoryDiscount { get; set; }
        public List<AccessoryCategory> AccessoryCategory { get; set; }
    }
}
