using BL.Contracts.Service.Base;
using Domains.Entities.Merchandising.HomePage;
using Shared.DTOs.HomeSlider;

namespace BL.Contracts.Service.HomeSlider
{
    public interface IHomePageSliderService : IBaseService<TbHomePageSlider, HomePageSliderDto>
    {
        /// <summary>
        /// Get all active sliders within current date range
        /// </summary>
        Task<IEnumerable<HomePageSliderDto>> GetAllSliders();
    }
}
