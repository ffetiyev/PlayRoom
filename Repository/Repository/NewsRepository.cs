using Domain.Models.News;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class NewsRepository : BaseRepository<News>,INewsRepository
    {
        private readonly AppDbContext _context;
        public NewsRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
