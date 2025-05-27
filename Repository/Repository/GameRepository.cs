using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
