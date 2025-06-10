using Service.ViewModels.Accessory;
using Service.ViewModels.Console;
using Service.ViewModels.Game;

namespace Service.ViewModels
{
    public class SearchVM
    {
        public List<GameVM> Games { get; set; }
        public List<ConsoleVM> Consoles { get; set; }
        public List<AccessoryVM> Accessories { get; set; }
    }
}
