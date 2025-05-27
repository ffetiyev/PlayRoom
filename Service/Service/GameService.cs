using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<IEnumerable<GameVM>> GetAllAsync()
        {
            var datas = await _gameRepository.GetAllAsync(null,m=>m.Include(p=>p.GameCategories).ThenInclude(gc=>gc.Category).Include(m=>m.GameImages).Include(m=>m.GameDiscounts).ThenInclude(gd=>gd.Discount));
           return datas.Select(m => new GameVM
            {
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                StockCount = m.StockCount,
                CreatedDate = m.CreatedDate,
                GameImages = m.GameImages.Select(i => new GameImageVM { IsMain = i.IsMain, Name = i.Name }).ToList(),
                GameDiscounts = m.GameDiscounts.Where(d => d.Discount != null).Select(d => new GameDiscountVM { Value = d.Discount.Value }).ToList(),
                GameCategory = m.GameCategories.Where(c => c.Category != null).Select(c => new GameCategoryVM {Id=c.Category.Id, Name = c.Category.Name }).ToList()
            }).ToList();

        }
    }
}
