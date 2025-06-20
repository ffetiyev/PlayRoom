using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.Promocode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class PromocodeService : IPromocodeService
    {
        private readonly IPromocodeRepository _promocodeRepo;
        public PromocodeService(IPromocodeRepository promocodeRepo)
        {
            _promocodeRepo = promocodeRepo;
        }
        public async Task CreateAsync(PromocodeCreateVM model)
        {
            await _promocodeRepo.CreateAsync(new Domain.Models.Promocode { Name = model.Name ,Value=model.Value});
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _promocodeRepo.GetByIdAsync(id);
            await _promocodeRepo.DeleteAsync(existData);
        }

        public async Task<IEnumerable<PromocodeVM>> GetAllAsync()
        {
            var datas =await _promocodeRepo.GetAllAsync();
            return datas.Select(x => new PromocodeVM { Id=x.Id, Name=x.Name, Value=x.Value });
        }

        public async Task<PromocodeVM> GetByIdAsync(int id)
        {
            var data = await _promocodeRepo.GetByIdAsync(id);
            return new PromocodeVM { Id=data.Id, Name=data.Name, Value=data.Value };
        }

        public async Task UpdateAsync(int id, PromocodeUpdateVM model)
        {
            var data = await _promocodeRepo.GetByIdAsync(id);
            data.Name = model.Name;
            data.Value = model.Value;
            await _promocodeRepo.UpdateAsync(data);

        }
    }
}
