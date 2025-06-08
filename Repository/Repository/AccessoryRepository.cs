using Domain.Models;
using Domain.Models.Accessory;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class AccessoryRepository : BaseRepository<Accessory>,IAccessoryRepository
    {
        private readonly AppDbContext _context;
        public AccessoryRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task AddCategoriesToAccessory(int accessoryId, IEnumerable<int> categoriIds)
        {
            await _context.AccessoryCategories.AddRangeAsync(categoriIds.Select(m => new AccessoryCategory
            {
                CategoryId = m,
                AccessoryId = accessoryId,
            }));
            await _context.SaveChangesAsync();
        }

        public async Task AddImagesToAccessory(IEnumerable<AccessoryImage> images)
        {
            await _context.AccessoryImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Accessory>> GetAllQueryable()
        {
            return _context.Accessories
                  .Include(m => m.AccessoryCategory).ThenInclude(m => m.Category)
                  .Include(m => m.AccessoryImage)
                  .Include(m => m.AccessoryDiscount).ThenInclude(m => m.Discount).AsQueryable();
        }
    }
}
