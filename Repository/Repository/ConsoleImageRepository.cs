using Domain.Models.Console;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class ConsoleImageRepository : BaseRepository<ConsoleImage>,IConsoleImageRepository
    {
        private readonly AppDbContext _context;
        public ConsoleImageRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task SetImageMain(int id)
        {
            var image = await _context.ConsoleImages.FirstOrDefaultAsync(m => m.Id == id);

            var console = await _context.Consoles.Include(m => m.ConsoleImages).FirstOrDefaultAsync(m => m.Id == image.ConsoleId);

            foreach (var item in console.ConsoleImages)
            {
                item.IsMain = false;
            }

            image.IsMain = true;

            await _context.SaveChangesAsync();
        }
    }
}
