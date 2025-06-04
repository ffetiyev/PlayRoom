using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class GameImageRepository : BaseRepository<GameImage>, IGameImageRepository
    {
        private readonly AppDbContext _context;
        public GameImageRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task SetImageMain(int id)
        {
            var image = await _context.GameImages.FirstOrDefaultAsync(m=>m.Id == id);

            var game = await _context.Games.Include(m=>m.GameImages).FirstOrDefaultAsync(m => m.Id == image.GameId);

            foreach (var item in game.GameImages)
            {
                item.IsMain = false;
            }
            image.IsMain=true;
            await _context.SaveChangesAsync();
        }
    }
}
