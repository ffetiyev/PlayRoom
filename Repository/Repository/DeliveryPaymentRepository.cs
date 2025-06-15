using Domain.Models;
using Repository.Data;
using Repository.Repository.Interfaces;

namespace Repository.Repository
{
    public class DeliveryPaymentRepository : BaseRepository<DeliveryPayment>,IDeliveryPaymentRepository
    {
        private readonly AppDbContext _context;
        public DeliveryPaymentRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
