using Domain.Commons;

namespace Domain.Models
{
    public class GameImage : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }

    }
}
