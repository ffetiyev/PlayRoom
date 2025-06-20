using Domain.Commons;

namespace Domain.Models.Console
{
    public class ConsoleImage : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int ConsoleId { get; set; }
        public Console Console { get; set; }
    }
}
