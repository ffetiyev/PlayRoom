using Domain.Commons;
using Domain.Models.Accessory;
using Domain.Models.Console;
using Domain.Models.Game;

namespace Domain.Models
{
    public class Discount : BaseEntity
    {
        public decimal Value { get; set; }
        public List<GameDiscount> GameDiscounts { get; set; }
        public List<ConsoleDiscount> ConsoleDiscounts { get; set; }
        public List<AccessoryDiscount> AccessoryDiscounts { get; set; }
    }
}
