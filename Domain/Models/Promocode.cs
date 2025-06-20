using Domain.Commons;

namespace Domain.Models
{
    public class Promocode : BaseEntity
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
