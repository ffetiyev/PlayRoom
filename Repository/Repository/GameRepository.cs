using Azure;
using Domain.Models;
using Domain.Models.Game;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;
using System.Collections.Immutable;

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

        public async Task<List<Game>> GetAllPaginated(int page, int take)
        {
            return await _context.Games
         .AsNoTracking()
         .Include(m => m.GameCategories).ThenInclude(m => m.Category)
         .Include(m => m.GameImages)
         .Include(m => m.GameDiscounts).ThenInclude(m=>m.Discount)
         .Skip((page - 1) * take)
         .Take(take)
         .ToListAsync();
        }

        public async Task<IQueryable<Game>> GetAllQueryable()
        {
            return _context.Games
                .Include(m => m.GameCategories).ThenInclude(m => m.Category)
                .Include(m => m.GameImages)
                .Include(m => m.GameDiscounts).ThenInclude(m => m.Discount);
        }
    }
}
