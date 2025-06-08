using Service.ViewModels.Category;
using Service.ViewModels.Discount;

namespace Service.ViewModels.Accessory
{
    public class AccessoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<AccessoryImageVM> Images { get; set; }
        public List<DiscountVM> Discounts { get; set; }
        public List<CategoryVM> Category { get; set; }
    }
}
