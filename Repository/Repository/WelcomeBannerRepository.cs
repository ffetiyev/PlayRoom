using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class WelcomeBannerRepository : BaseRepository<WelcomeBanner>,IWelcomeBannerRepository
    {
        private readonly AppDbContext _context;
        public WelcomeBannerRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<WelcomeBanner> GetAsync()
        {
            return await _context.WelcomeBanner.FirstAsync();
        }
    }
}
