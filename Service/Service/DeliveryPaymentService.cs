using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels;

namespace Service.Service
{
    class DeliveryPaymentService : IDeliveryPaymentService
    {
        private readonly IDeliveryPaymentRepository _repository;
        public DeliveryPaymentService(IDeliveryPaymentRepository repository)
        {
            _repository = repository;
        }
        public async Task<DeliveryPaymentVM> GetAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(x => new DeliveryPaymentVM {Content = x.Content,Id=x.Id,Title=x.Title}).FirstOrDefault();
        }

        public async Task<DeliveryPaymentVM> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return new DeliveryPaymentVM { Content = data.Content, Id = data.Id,Title=data.Title };
        }

        public async Task UpdateAsync(int id, DeliveryPaymentVM model)
        {
           var existData = await _repository.GetByIdAsync(id);
            if(model.Content != null) existData.Content = model.Content;
            if (model.Title != null) existData.Title = model.Title;
            await _repository.UpdateAsync(existData);
        }
    }
}
