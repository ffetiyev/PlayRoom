using Domain.Models;
using Domain.Models.Game;

namespace Repository.Repository.Interfaces
{
    public interface IGameRepository:IBaseRepository<Game>
    {
        Task AddImagesToGame(IEnumerable<GameImage> images);
        Task AddDiscountToGame(int gameId, IEnumerable<Discount> discounts);
        Task AddCategoriesToGame(int gameId, IEnumerable<int> categoriIds);
        Task<List<Game>> GetAllPaginated(int page, int take);
        Task<IQueryable<Game>> GetAllQueryable();
    }
}
