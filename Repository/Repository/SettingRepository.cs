using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class SettingRepository : BaseRepository<Setting>,ISettingRepository
    {
        private readonly AppDbContext _context;
        public SettingRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
