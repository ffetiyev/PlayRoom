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
    }
}

