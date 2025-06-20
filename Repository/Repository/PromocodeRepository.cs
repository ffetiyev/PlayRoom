using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class PromocodeRepository : BaseRepository<Promocode>,IPromocodeRepository
    {
        private readonly AppDbContext _context;
        public PromocodeRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
