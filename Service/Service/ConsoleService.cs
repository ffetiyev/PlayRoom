using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Category;
using Service.ViewModels.Console;
using Service.ViewModels.Discount;
using Domain.Models;
using Repository.Repository;

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
            Domain.Models.Console console= new ()
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
    }
}
