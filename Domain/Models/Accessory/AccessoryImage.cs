using Domain.Commons;

namespace Domain.Models.Accessory
{
    public class AccessoryImage : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int AccessoryId { get; set; }
        public Accessory Accessory { get; set; }
    }
}
