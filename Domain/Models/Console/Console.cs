using Domain.Commons;

namespace Domain.Models.Console
{
    public class Console : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public int Memory { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<ConsoleImage> ConsoleImages { get; set; }
        public List<ConsoleDiscount> ConsoleDiscounts { get; set; }
        public List<ConsoleCategory> ConsoleCategories { get; set; }

    }
}
