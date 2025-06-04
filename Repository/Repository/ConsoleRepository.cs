using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class ConsoleRepository : BaseRepository<Domain.Models.Console>,IConsoleRepository
    {
        private readonly AppDbContext _context;
        public ConsoleRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task AddCategoriesToConsole(int consoleId, IEnumerable<int> categoriIds)
        {
            await _context.ConsoleCategories.AddRangeAsync(categoriIds.Select(m => new ConsoleCategory
            {
                CategoryId = m,
                ConsoleId = consoleId,
            }));
            await _context.SaveChangesAsync();
        }
        public async Task AddImagesToConsole(IEnumerable<ConsoleImage> images)
        {
            await _context.ConsoleImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Domain.Models.Console>> GetAllQueryable()
        {
            return _context.Consoles
                .Include(m => m.ConsoleCategories).ThenInclude(m => m.Category)
                .Include(m => m.ConsoleImages)
                .Include(m => m.ConsoleDiscounts).ThenInclude(m => m.Discount);
        }
    }
}
