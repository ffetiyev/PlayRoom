using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class WarrantyRepository : BaseRepository<Warranty>,IWarrantyRepository
    {
        private readonly AppDbContext _context;
        public WarrantyRepository(AppDbContext context):base(context) 
        {
            _context = context;
        }
    }
}
