using AutoMapper;
using Repository.Repository.Interfaces;
using Service.Service.Interfaces;
using Service.ViewModels.News;

namespace Service.Service
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        public NewsService(INewsRepository newsRepository,
                           IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(NewsCreateVM model)
        {
            await _newsRepository.CreateAsync(new()
            {
                Description = model.Description,
                Image = model.ImageName,
                Title = model.Title,
                VideoLink=model.VideoLink
            });
        }

        public async Task DeleteAsync(int id)
        {
            var existData = await _newsRepository.GetByIdAsync(id);
            await _newsRepository.DeleteAsync(existData);
        }

        public async Task<IEnumerable<NewsVM>> GetAllAsync()
        {
            var datas = await _newsRepository.GetAllAsync();
            var sortedDatas=datas.OrderByDescending(x => x.CreatedDate);
           return _mapper.Map<IEnumerable<NewsVM>>(sortedDatas);
        }

        public async Task<NewsVM> GetByIdAsync(int id)
        {
           return _mapper.Map<NewsVM>(await _newsRepository.GetByIdAsync(id));
        }

        public async Task UpdateAsync(int id, NewsUpdateVM model)
        {
            var existData = await _newsRepository.GetByIdAsync(id);
            if(model.Description != null) existData.Description = model.Description;
            if(model.Title!=null ) existData.Title = model.Title;
            if(model.Image!=null) existData.Image = model.Image;
            existData.VideoLink = model.VideoLink;
            await _newsRepository.UpdateAsync(existData);
        }
    }
}
