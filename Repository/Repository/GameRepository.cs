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

        public async Task AddCategoriesToGame(int gameId, IEnumerable<int> categoriIds)
        {
            await _context.GameCategories.AddRangeAsync(categoriIds.Select(m=>new GameCategory
            {
                CategoryId=m,
                GameId=gameId,
            }));
            await _context.SaveChangesAsync();
        }

        public async Task AddDiscountToGame(int gameId, IEnumerable<Discount> discounts)
        {
            await _context.GameDiscounts.AddRangeAsync(discounts.Select(m=>new GameDiscount
            {
                DiscountId = m.Id,
                GameId = gameId,
            }));
            await _context.SaveChangesAsync();
        }

        public async Task AddImagesToGame(IEnumerable<GameImage> images)
        {
            await _context.GameImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
    }
}
