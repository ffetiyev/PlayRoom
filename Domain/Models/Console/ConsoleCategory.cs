using Domain.Commons;

namespace Domain.Models.Console
{
    public class ConsoleCategory:BaseEntity
    {
        public int ConsoleId { get; set; }
        public Console Console { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
