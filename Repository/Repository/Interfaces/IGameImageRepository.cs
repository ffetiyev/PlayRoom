using Domain.Models.Game;

namespace Repository.Repository.Interfaces
{
    public interface IGameImageRepository : IBaseRepository<GameImage>
    {
        Task SetImageMain(int id);
    }
}
