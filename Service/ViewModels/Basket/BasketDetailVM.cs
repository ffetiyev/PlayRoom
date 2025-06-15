using Service.ViewModels.Accessory;
using Service.ViewModels.Console;
using Service.ViewModels.Game;

namespace Service.ViewModels.Basket
{
    public class BasketDetailVM
    {
        public Dictionary<GameVM,int> Games { get; set; }
        public Dictionary<ConsoleVM, int> Consoles { get; set; }
        public Dictionary<AccessoryVM, int> Accessories { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDiscountless { get; set; }
    }
}
