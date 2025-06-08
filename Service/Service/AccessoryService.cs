using Domain.Models.Accessory;
using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Helpers.Responses;
using Service.Service.Interfaces;
using Service.ViewModels.Accessory;
using Service.ViewModels.Category;
using Service.ViewModels.Console;
using Service.ViewModels.Discount;

namespace Service.Service
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IAccessoryRepository _accessoryRepo;
        public AccessoryService(IAccessoryRepository accessoryRepo)
        {
            _accessoryRepo = accessoryRepo;
        }
        public async Task CreateAsync(AccessoryCreateVM model)
        {
            var accessory = new Accessory()
            {
                Description = model.Description,
                Name = model.Name,
                Price = (decimal)model.Price,
                StockCount = (int)model.StockCount,
                CreatedDate = DateTime.UtcNow,
                AccessoryImage = model.Images.Select(m => new AccessoryImage
                {
                    IsMain = m.IsMain,
                    Name = m.Name
                }).ToList()
            };

            await _accessoryRepo.CreateAsync(accessory);

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                await _accessoryRepo.AddCategoriesToAccessory(accessory.Id, model.CategoryIds);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _accessoryRepo.GetByIdAsync(id);
            await _accessoryRepo.DeleteAsync(existData);
        }

        public async Task<IEnumerable<AccessoryVM>> GetAllAsync()
        {
            var datas = await _accessoryRepo.GetAllAsync(null, m => m.Include(p => p.AccessoryCategory).ThenInclude(gc => gc.Category).Include(m => m.AccessoryImage).Include(m => m.AccessoryDiscount).ThenInclude(gd => gd.Discount));
            return datas.Select(m => new AccessoryVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                Images = m.AccessoryImage.Select(i => new AccessoryImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                Discounts = m.AccessoryDiscount.Where(d => d.Discount != null).Select(d => new DiscountVM { Value = d.Discount.Value }).ToList(),
                Category = m.AccessoryCategory.Where(c => c.Category != null).Select(c => new CategoryVM { Id = c.Category.Id, Name = c.Category.Name }).ToList()
            }).ToList();
        }

        public async Task<PaginateResponse<AccessoryVM>> GetAllPaginated(int page, int take = 8, string? category = null, string? priceRange = null, string? orderBy = null)
        {
            var datas =await  _accessoryRepo.GetAllQueryable();
            var paginatedDatas = datas.Select(m => new AccessoryVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                Images = m.AccessoryImage.Select(i => new AccessoryImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                Discounts = m.AccessoryDiscount.Where(d => d.Discount != null).Select(d => new DiscountVM { Value = d.Discount.Value }).ToList(),
                Category = m.AccessoryCategory.Where(c => c.Category != null).Select(c => new CategoryVM { Id = c.Category.Id, Name = c.Category.Name }).ToList()
            }).ToList();

            if (category != null)
            {
                paginatedDatas = paginatedDatas.Where(m => m.Category.Any(gc => gc.Name == category)).ToList();
            }
            if (priceRange != null)
            {
                switch (priceRange)
                {
                    case "100":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 0 && m.Price < 100).ToList();
                        break;
                    case "200":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 100 && m.Price < 200).ToList();
                        break;
                    case "500":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 200 && m.Price < 500).ToList();
                        break;
                    case "1000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 500 && m.Price < 1000).ToList();
                        break;
                    case "10000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 1000 && m.Price < 10000).ToList();
                        break;
                    default:
                        paginatedDatas = paginatedDatas;
                        break;
                }
            }
            if (orderBy != null)
            {
                switch (orderBy)
                {
                    case "new":
                        paginatedDatas = paginatedDatas.OrderByDescending(m => m.CreatedDate).ToList();
                        break;
                    case "cheap":
                        paginatedDatas = paginatedDatas
                            .OrderBy(m => m.Discounts
                                .Aggregate(m.Price, (current, discount) => current * (1 - discount.Value / 100m)))
                            .ToList();
                        break;

                    case "expensive":
                        paginatedDatas = paginatedDatas
                            .OrderByDescending(m => m.Discounts
                                .Aggregate(m.Price, (current, discount) => current * (1 - discount.Value / 100m)))
                            .ToList();
                        break;
                    default:
                        break;
                }
            }
            int count = paginatedDatas.Count();
            if (category == null && priceRange == null && orderBy == null)
            {
                count = await _accessoryRepo.GetCountAsync();
            }
            int totalPage = (int)Math.Ceiling((decimal)count / take);

            return new PaginateResponse<AccessoryVM>(paginatedDatas.Skip((page - 1) * take).Take(take).ToList(), page, totalPage);
        }

        public async Task<AccessoryVM> GetByIdAsync(int id)
        {
            var data = await _accessoryRepo.GetByIdAsync(id, m => m.Include(p => p.AccessoryCategory).ThenInclude(gc => gc.Category).Include(m => m.AccessoryImage).Include(m => m.AccessoryDiscount).ThenInclude(gd => gd.Discount));
            if (data == null) return null;
            return new AccessoryVM
            {
                Id = data.Id,
                Description = data.Description,
                CreatedDate = data.CreatedDate,
                Name = data.Name,
                Price = data.Price,
                StockCount = data.StockCount,
                Category = data.AccessoryCategory.Select(m => new CategoryVM { Id = m.Category.Id, Name = m.Category.Name }).ToList(),
                Discounts = data.AccessoryDiscount.Select(m => new DiscountVM { Id = m.Discount.Id, Value = m.Discount.Value }).ToList(),
                Images = data.AccessoryImage.Select(m => new AccessoryImageVM { Id = m.Id, Name = m.Name, IsMain = m.IsMain }).ToList(),
            };
        }

        public async Task UpdateAsync(int id, AccessoryUpdateVM model)
        {
            var existData = await _accessoryRepo.GetByIdAsync(id,
                m => m.Include(p => p.AccessoryCategory)
                      .ThenInclude(gc => gc.Category)
                      .Include(m => m.AccessoryImage)
                      .Include(m => m.AccessoryDiscount)
                      .ThenInclude(gd => gd.Discount));

            if (model.Images != null)
            {
                await _accessoryRepo.AddImagesToAccessory(model.Images.Select(m => new AccessoryImage
                {
                    AccessoryId = id,
                    IsMain = m.IsMain,
                    Name = m.Name
                }).ToList());
            }

            existData.Name = model.NewName;
            existData.Description = model.NewDescription;
            existData.Price = (decimal)model.NewPrice;
            existData.StockCount = (int)model.NewStockCount;
            model.SelectedDiscountIds ??= new List<int>();
            existData.AccessoryDiscount.RemoveAll(d => !model.SelectedDiscountIds.Contains(d.DiscountId));

            var existingDiscountIds = existData.AccessoryDiscount.Select(d => d.DiscountId).ToList();
            foreach (var discountId in model.SelectedDiscountIds)
            {
                if (!existingDiscountIds.Contains(discountId))
                {
                    existData.AccessoryDiscount.Add(new AccessoryDiscount
                    {
                        AccessoryId = existData.Id,
                        DiscountId = discountId
                    });
                }
            }

            model.SelectedCategoryIds ??= new List<int>();
            existData.AccessoryCategory.RemoveAll(c => !model.SelectedCategoryIds.Contains(c.CategoryId));

            var existingCategoryIds = existData.AccessoryCategory.Select(c => c.CategoryId).ToList();
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (!existingCategoryIds.Contains(categoryId))
                {
                    existData.AccessoryCategory.Add(new AccessoryCategory
                    {
                        AccessoryId = existData.Id,
                        CategoryId = categoryId
                    });
                }
            }

            await _accessoryRepo.UpdateAsync(existData);
        }

    }
}
