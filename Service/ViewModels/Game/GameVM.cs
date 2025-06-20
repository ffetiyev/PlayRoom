﻿using Service.ViewModels.Category;
using Service.ViewModels.Discount;

namespace Service.ViewModels.Game
{
    public class GameVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<GameImageVM> GameImages { get; set; }
        public List<DiscountVM> GameDiscounts { get; set; }
        public List<CategoryVM> GameCategory { get; set; }
    }
}



