using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class HomeShortcutRepository : BaseRepository<HomeShortcut>,IHomeShortcutRepository
    {
        private readonly AppDbContext _context;
        public HomeShortcutRepository(AppDbContext context):base(context) 
        {
            _context = context;
        }
    }
}
