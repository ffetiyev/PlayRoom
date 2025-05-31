using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Helpers.Responses;
using Service.Service.Interfaces;
using Service.ViewModels;
using Service.ViewModels.Category;
using Service.ViewModels.Game;

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
            }).ToList();

        }

        public async Task<PaginateResponse<GameVM>> GetAllPaginated(int page, int take = 16)
        {
            var datas = await _gameRepository.GetAllPaginated(page, take);
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

            int count = await _gameRepository.GetCountAsync();

            int totalPage =(int)Math.Ceiling((decimal)count / take);

            return new PaginateResponse<GameVM>(paginatedDatas, page,totalPage);
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
                GameImages = data.GameImages.Select(m => new GameImageVM { Id = m.Id, Name = m.Name, IsMain = true }).ToList(),
            };
        }

        public async Task UpdateAsync(int id, GameUpdateVM model)
        {
            var existData = await _gameRepository.GetByIdAsync(id, m => m.Include(p => p.GameCategories).ThenInclude(gc => gc.Category).Include(m => m.GameImages).Include(m => m.GameDiscounts).ThenInclude(gd => gd.Discount));
            if (model.Images != null) await _gameRepository.AddImagesToGame(model.Images.Select(m=>new GameImage {GameId=id, IsMain=m.IsMain,Name=m.Name}).ToList());
            existData.Name = model.Name;
            existData.Description = model.Description;
            existData.Price = model.Price;
            existData.StockCount = model.StockCount;
            if (model.GameDiscounts != null) await _gameRepository.AddDiscountToGame(id, model.GameDiscounts.Select(m => new Discount { Id = m.Id }));
            if(model.CategorieIds!=null) await _gameRepository.AddCategoriesToGame(id,model.CategorieIds);
            

        }
    }
}
