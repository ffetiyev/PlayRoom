using Domain.Models;

namespace Repository.Repository.Interfaces
{
    public interface IConsoleImageRepository : IBaseRepository<ConsoleImage>
    {
        Task SetImageMain(int id);
    }
}
