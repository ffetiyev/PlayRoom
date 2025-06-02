using Service.Helpers.Responses;
using Service.ViewModels.Game;

namespace Service.Service.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameVM>> GetAllAsync();
        Task CreateAsync(GameCreateVM model);
        Task<GameVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, GameUpdateVM model);
        Task<PaginateResponse<GameVM>> GetAllPaginated(int page, int take = 16, string? category = null, string? priceRange = null, string? orderBy = null);
    }
}

