using Domain.Commons;

namespace Domain.Models
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string? Value { get; set; }
    }
}
