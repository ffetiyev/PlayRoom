using Domain.Models.Accessory;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class AccessoryImageRepository : BaseRepository<AccessoryImage>,IAccessoryImageRepository
    {
        private readonly AppDbContext _context;
        public AccessoryImageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task SetImageMain(int id)
        {
            var image = await _context.AccessoryImages.FirstOrDefaultAsync(m => m.Id == id);

            var accessory = await _context.Accessories.Include(m => m.AccessoryImage).FirstOrDefaultAsync(m => m.Id == image.AccessoryId);

            foreach (var item in accessory.AccessoryImage)
            {
                item.IsMain = false;
            }

            image.IsMain = true;

            await _context.SaveChangesAsync();
        }
    }
}
