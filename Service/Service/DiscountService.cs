using AutoMapper;
using Domain.Models;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Discount;

namespace Service.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository repository,
                               IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(DiscountCreateVM request)
        {
            await _repository.CreateAsync(_mapper.Map<Discount>(request));
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(existData);
        }

        public async Task<IEnumerable<DiscountVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<DiscountVM>>(await _repository.GetAllAsync());
        }

        public async Task<DiscountVM> GetByIdAsync(int id)
        {
            var existDiscount = await _repository.GetByIdAsync(id);
            return _mapper.Map<DiscountVM>(existDiscount);
        }

        public async Task UpdateAsync(int id, DiscountUpdateVM model)
        {
            var existDiscount = await _repository.GetByIdAsync(id);
            if(model.Value!=null) existDiscount.Value=(int)model.Value;

            await _repository.UpdateAsync(existDiscount);
        }
    }
}
