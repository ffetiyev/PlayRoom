using Domain.Models;
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
    }
}
