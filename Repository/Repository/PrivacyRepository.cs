using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class PrivacyRepository : BaseRepository<Privacy>,IPrivacyRepository
    {
        private readonly AppDbContext _context;
        public PrivacyRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
