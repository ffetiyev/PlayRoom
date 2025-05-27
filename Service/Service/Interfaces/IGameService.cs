using Service.ViewModels;

namespace Service.Service.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameVM>> GetAllAsync();
    }
}
