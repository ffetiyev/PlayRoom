using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Helpers.Responses;
using Service.Service.Interfaces;
using Service.ViewModels;
using Service.ViewModels.Category;
using Service.ViewModels.Game;
using System.Linq;

namespace Service.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        public GameService(IGameRepository gameRepository,IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(GameCreateVM model)
        {
            var game = new Game
            {
                Description = model.Description,
                Name = model.Name,
                Price = (decimal)model.Price,
                StockCount = (int)model.StockCount,
                CreatedDate = DateTime.UtcNow,
                GameImages = model.Images.Select(m => new GameImage
                {
                    IsMain = m.IsMain,
                    Name = m.Name
                }).ToList()
            };

            await _gameRepository.CreateAsync(game);

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                await _gameRepository.AddCategoriesToGame(game.Id, model.CategoryIds);
            }
        }



        public async Task DeleteAsync(int id)
        {
            var existData = await _gameRepository.GetByIdAsync(id);
            await _gameRepository.DeleteAsync(existData);
        }

        public async Task<IEnumerable<GameVM>> GetAllAsync()
        {
            var datas = await _gameRepository.GetAllAsync(null,m=>m.Include(p=>p.GameCategories).ThenInclude(gc=>gc.Category).Include(m=>m.GameImages).Include(m=>m.GameDiscounts).ThenInclude(gd=>gd.Discount));
           return datas.Select(m => new GameVM
            {
               Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                GameImages = m.GameImages.Select(i => new GameImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                GameDiscounts = m.GameDiscounts.Where(d => d.Discount != null).Select(d => new GameDiscountVM { Value = d.Discount.Value }).ToList(),
                GameCategory = m.GameCategories.Where(c => c.Category != null).Select(c => new CategoryVM {Id=c.Category.Id, Name = c.Category.Name }).ToList()
            }).AsQueryable();

        }

        public async Task<PaginateResponse<GameVM>> GetAllPaginated(int page, int take = 16, string? category=null, string? priceRange=null, string? orderBy=null)
        {
            var datas =await _gameRepository.GetAllQueryable();
            var paginatedDatas = datas.Select(m => new GameVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,  
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                GameImages = m.GameImages.Select(i => new GameImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                GameDiscounts = m.GameDiscounts.Where(d => d.Discount != null).Select(d => new GameDiscountVM { Value = d.Discount.Value }).ToList(),
                GameCategory = m.GameCategories.Where(c => c.Category != null).Select(c => new CategoryVM { Id = c.Category.Id, Name = c.Category.Name }).ToList()
            }).ToList();
            
            if(category != null)
            {
                paginatedDatas = paginatedDatas.Where(m => m.GameCategory.Any(gc => gc.Name == category)).ToList();
            }
            if (priceRange != null)
            {
                switch (priceRange)
                {
                    case "50":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 0 && m.Price < 50).ToList();
                        break;
                    case "100":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 50 && m.Price < 100).ToList();
                        break;
                    case "150":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 100 && m.Price < 150).ToList();
                        break;
                    case "1000":
                        paginatedDatas = paginatedDatas.Where(m => m.Price > 150 && m.Price < 1000).ToList();
                        break;
                    default:
                        paginatedDatas = paginatedDatas;
                        break;
                }
            }
            if(orderBy!= null)
            {
                switch (orderBy)
                {
                    case "new":
                        paginatedDatas = paginatedDatas.OrderByDescending(m=>m.CreatedDate).ToList(); 
                        break;
                    case "cheap":
                        paginatedDatas = paginatedDatas.OrderBy(m => m.Price).ToList();
                        break;
                    case "expensive":
                        paginatedDatas = paginatedDatas.OrderByDescending(m => m.Price).ToList();
                        break;
                    default:
                        break;
                }
            }
             int count = paginatedDatas.Count();
            if (category==null && priceRange ==null && orderBy==null)
            {
                 count = await _gameRepository.GetCountAsync();
            }
            int totalPage =(int)Math.Ceiling((decimal)count / take);

            return new PaginateResponse<GameVM>(paginatedDatas.Skip((page - 1) * take).Take(take).ToList(), page,totalPage);
        }

        public async Task<GameVM> GetByIdAsync(int id)
        {
           var data = await _gameRepository.GetByIdAsync(id,m=>m.Include(p=>p.GameCategories).ThenInclude(gc=>gc.Category).Include(m => m.GameImages).Include(m => m.GameDiscounts).ThenInclude(gd => gd.Discount));
            if (data == null) return null;
            return new GameVM
            {
                Description = data.Description,
                CreatedDate = data.CreatedDate,
                Name = data.Name,
                Price = data.Price,
                StockCount = data.StockCount,
                GameCategory = data.GameCategories.Select(m => new CategoryVM { Id = m.Category.Id, Name = m.Category.Name }).ToList(),
                GameDiscounts = data.GameDiscounts.Select(m => new GameDiscountVM { Id = m.Discount.Id, Value = m.Discount.Value }).ToList(),
                GameImages = data.GameImages.Select(m => new GameImageVM { Id = m.Id, Name = m.Name, IsMain = m.IsMain }).ToList(),
            };
        }

        public async Task UpdateAsync(int id, GameUpdateVM model)
        {
            var existData = await _gameRepository.GetByIdAsync(id,
                m => m.Include(p => p.GameCategories)
                      .ThenInclude(gc => gc.Category)
                      .Include(m => m.GameImages)
                      .Include(m => m.GameDiscounts)
                      .ThenInclude(gd => gd.Discount));

            if (model.Images != null)
            {
                await _gameRepository.AddImagesToGame(model.Images.Select(m => new GameImage
                {
                    GameId = id,
                    IsMain = m.IsMain,
                    Name = m.Name
                }).ToList());
            }

            existData.Name = model.NewName;
            existData.Description = model.NewDescription;
            existData.Price = (decimal)model.NewPrice;
            existData.StockCount = (int)model.NewStockCount;
            model.SelectedDiscountIds ??= new List<int>(); 
            existData.GameDiscounts.RemoveAll(d => !model.SelectedDiscountIds.Contains(d.DiscountId));

            var existingDiscountIds = existData.GameDiscounts.Select(d => d.DiscountId).ToList();
            foreach (var discountId in model.SelectedDiscountIds)
            {
                if (!existingDiscountIds.Contains(discountId))
                {
                    existData.GameDiscounts.Add(new GameDiscount
                    {
                        GameId = existData.Id,
                        DiscountId = discountId
                    });
                }
            }

            model.SelectedCategoryIds ??= new List<int>();
            existData.GameCategories.RemoveAll(c => !model.SelectedCategoryIds.Contains(c.CategoryId));

            var existingCategoryIds = existData.GameCategories.Select(c => c.CategoryId).ToList();
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (!existingCategoryIds.Contains(categoryId))
                {
                    existData.GameCategories.Add(new GameCategory
                    {
                        GameId = existData.Id,
                        CategoryId = categoryId
                    });
                }
            }

            await _gameRepository.UpdateAsync(existData);
        }

    }
}
