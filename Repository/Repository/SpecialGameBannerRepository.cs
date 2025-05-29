using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class SpecialGameBannerRepository : BaseRepository<SpecialGameBanner>,ISpecialGameBannerRepository
    {
        private readonly AppDbContext _context;
        public SpecialGameBannerRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task SetActiveBannerAsync(int id)
        {
            foreach (var item in await _context.SpecialGameBanner.ToListAsync())
            {
                item.IsActive = false;
            }
            var data = await _context.SpecialGameBanner.FirstOrDefaultAsync(m=>m.Id == id);
            data.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }
}
