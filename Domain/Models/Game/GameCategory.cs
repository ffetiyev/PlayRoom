using Domain.Commons;

namespace Domain.Models.Game
{
    public class GameCategory : BaseEntity
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
