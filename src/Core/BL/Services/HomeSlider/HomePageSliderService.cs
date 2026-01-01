using BL.Contracts.IMapper;
using BL.Contracts.Service.HomeSlider;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.Merchandising.HomePage;
using Shared.DTOs.HomeSlider;

namespace BL.Services.HomeSlider
{
    public class HomePageSliderService : BaseService<TbHomePageSlider, HomePageSliderDto>, IHomePageSliderService
    {
        private readonly ITableRepository<TbHomePageSlider> _baseRepository;
        private readonly IBaseMapper _mapper;

        public HomePageSliderService(ITableRepository<TbHomePageSlider> baseRepository, IBaseMapper mapper)
            : base(baseRepository, mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all active sliders within current date range
        /// </summary>
        public async Task<IEnumerable<HomePageSliderDto>> GetAllSliders()
        {
            var sliders = await _baseRepository
                .GetAsync(x => !x.IsDeleted,
                orderBy: x => x.OrderBy(q => q.DisplayOrder));

            var dtos = _mapper.MapList<TbHomePageSlider, HomePageSliderDto>(sliders);

            return dtos;
        }
    }
}