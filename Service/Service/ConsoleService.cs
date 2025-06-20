using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Category;
using Service.ViewModels.Console;
using Service.ViewModels.Discount;
using Service.Helpers.Responses;
using Domain.Models.Console;


namespace Service.Service
{
    public class ConsoleService : IConsoleService
    {
        private readonly IConsoleRepository _repository;
        public ConsoleService(IConsoleRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(ConsoleCreateVM model)
        {
            Domain.Models.Console.Console console= new ()
            {
                Memory = (int)model.Memory,
                Name = model.Name,
                Price=(decimal)model.Price,
                Description = model.Description,
                CreatedDate= DateTime.UtcNow,
                StockCount=(int)model.StockCount,
                ConsoleImages=model.Images.Select(m=>new ConsoleImage() { IsMain = m.IsMain,Name=m.Name}).ToList(),
            };
            await _repository.CreateAsync(console);
            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                await _repository.AddCategoriesToConsole(console.Id, model.CategoryIds);
            }
        }

        public async Task<PaginateResponse<ConsoleVM>> GetAllPaginated(int page, List<string>? category, int take = 8, string? priceRange = null, string? orderBy = null)
        {
            var datas = _repository.GetAllQueryable();
            var paginatedDatas = datas.Select(m => new ConsoleVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                Memory = m.Memory,
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                Images = m.ConsoleImages.Select(i => new ConsoleImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                Discounts = m.ConsoleDiscounts.Where(d => d.Discount != null).Select(d => new DiscountVM { Value = d.Discount.Value }).ToList(),
                Categories = m.ConsoleCategories.Where(c => c.Category != null).Select(c => new CategoryVM { Id = c.Category.Id, Name = c.Category.Name }).ToList()
            }).ToList();

            if (category != null && category.Any())
            {
                paginatedDatas = paginatedDatas
                    .Where(m => m.Categories.Any(gc => category.Contains(gc.Name)))
                    .ToList();
            }

            if (priceRange != null)
            {
                switch (priceRange)
                {
                    case "300":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 0 && m.Price < 300).ToList();
                        break;
                    case "500":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 300 && m.Price < 500).ToList();
                        break;
                    case "1000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 500 && m.Price < 1000).ToList();
                        break;
                    case "2000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 1000 && m.Price < 2000).ToList();
                        break;
                    case "10000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 2000 && m.Price < 10000).ToList();
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
                count = await _repository.GetCountAsync();
            }
            int totalPage = (int)Math.Ceiling((decimal)count / take);

            return new PaginateResponse<ConsoleVM>(paginatedDatas.Skip((page - 1) * take).Take(take).ToList(), page, totalPage);
        }


        public async Task DeleteAsync(int id)
        {
            var existData = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(existData);
        }

        public async Task<IEnumerable<ConsoleVM>> GetAllAsync()
        {
            var datas  = await _repository.GetAllAsync(null,m=>m.Include(m=>m.ConsoleCategories).ThenInclude(m=>m.Category)
                                                                .Include(m=>m.ConsoleImages)
                                                                .Include(m=>m.ConsoleDiscounts).ThenInclude(m=>m.Discount));
            return datas.Select(x => new ConsoleVM()
            {
                Categories=x.ConsoleCategories.Select(m=>new CategoryVM {Name= m.Category.Name,Id= m.Category.Id}).ToList(),
                Discounts=x.ConsoleDiscounts.Select(m=>new DiscountVM { Id=m.Discount.Id,Value=m.Discount.Value}).ToList(),
                Images=x.ConsoleImages.Select(m=>new ConsoleImageVM { Id=x.Id,IsMain=m.IsMain,Name=m.Name}).ToList(),
                CreatedDate=x.CreatedDate,
                Description=x.Description,
                Id=x.Id,
                Memory=x.Memory,
                Name=x.Name,
                Price=x.Price,
                StockCount=x.StockCount,                
                
            }).ToList();
        }

        public async Task<ConsoleVM> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id,m=>m.Include(m => m.ConsoleCategories).ThenInclude(m => m.Category)
                                                                .Include(m => m.ConsoleImages)
                                                                .Include(m => m.ConsoleDiscounts).ThenInclude(m => m.Discount));
            if (data == null) return null;
            return new ConsoleVM()
            {
                Id = id,
                Name = data.Name,
                Memory = data.Memory,
                Price = data.Price,
                StockCount = data.StockCount,
                Description = data.Description,
                Categories = data.ConsoleCategories.Select(c => new CategoryVM { Id = c.Category.Id, Name = c.Category.Name }).ToList(),
                CreatedDate = data.CreatedDate,
                Discounts = data.ConsoleDiscounts.Select(c => new DiscountVM { Id = c.Discount.Id, Value = c.Discount.Value }).ToList(),
                Images = data.ConsoleImages.Select(c => new ConsoleImageVM { Id = c.Id, IsMain = c.IsMain, Name = c.Name }).ToList(),
            };
        }

        public async Task UpdateAsync(int id, ConsoleUpdateVM model)
        {
            var existData = await _repository.GetByIdAsync(id,
                m => m.Include(p => p.ConsoleCategories)
                      .ThenInclude(gc => gc.Category)
                      .Include(m => m.ConsoleImages)
                      .Include(m => m.ConsoleDiscounts)
                      .ThenInclude(gd => gd.Discount));

            if (model.Images != null)
            {
                await _repository.AddImagesToConsole(model.Images.Select(m => new ConsoleImage
                {
                    ConsoleId = id,
                    IsMain = m.IsMain,
                    Name = m.Name
                }).ToList());
            }

            existData.Name = model.NewName;
            existData.Description = model.NewDescription;
            existData.Price = (decimal)model.NewPrice;
            existData.Memory=(int)model.NewMemory;
            existData.StockCount = (int)model.NewStockCount;
            model.SelectedDiscountIds ??= new List<int>();
            existData.ConsoleDiscounts.RemoveAll(d => !model.SelectedDiscountIds.Contains(d.DiscountId));

            var existingDiscountIds = existData.ConsoleDiscounts.Select(d => d.DiscountId).ToList();
            foreach (var discountId in model.SelectedDiscountIds)
            {
                if (!existingDiscountIds.Contains(discountId))
                {
                    existData.ConsoleDiscounts.Add(new ConsoleDiscount
                    {
                        ConsoleId= existData.Id,
                        DiscountId = discountId
                    });
                }
            }

            model.SelectedCategoryIds ??= new List<int>();
            existData.ConsoleCategories.RemoveAll(c => !model.SelectedCategoryIds.Contains(c.CategoryId));

            var existingCategoryIds = existData.ConsoleCategories.Select(c => c.CategoryId).ToList();
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (!existingCategoryIds.Contains(categoryId))
                {
                    existData.ConsoleCategories.Add(new ConsoleCategory
                    {
                        ConsoleId = existData.Id,
                        CategoryId = categoryId
                    });
                }
            }

            await _repository.UpdateAsync(existData);
        }

    }
}
