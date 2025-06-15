using Domain.Models;
using Repository.Data;
using Service.ViewModels;

namespace Service.Service.Interfaces
{
   public interface IDeliveryPaymentService
    {
        Task<DeliveryPaymentVM> GetAsync();
        Task UpdateAsync(int id,DeliveryPaymentVM model);
        Task<DeliveryPaymentVM> GetByIdAsync(int id);
    }
}
