using Service.ViewModels.Category;
using Service.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Console
{
    public class ConsoleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public int Memory { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ConsoleImageVM> Images { get; set; }
        public List<DiscountVM> Discounts { get; set; }
        public List<CategoryVM> Categories { get; set; }
    }
}
