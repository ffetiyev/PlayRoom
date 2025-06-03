using Domain.Models;
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
    }
}
