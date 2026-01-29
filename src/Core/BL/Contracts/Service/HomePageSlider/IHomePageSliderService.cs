using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.Merchandising.HomePage;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.HomePageSlider
{
    public interface IHomePageSliderService : IBaseService<TbHomePageSlider, HomePageSliderDto>
    {
        /// <summary>
        /// Get all active sliders within current date range
        /// </summary>
        Task<IEnumerable<HomePageSliderDto>> GetAllSliders();
		Task<PagedResult<HomePageSliderDto>> GetPage(
			BaseSearchCriteriaModel criteriaModel,
			CancellationToken cancellationToken = default);
		Task<bool> Save(HomePageSliderDto dto, Guid userId);
		Task<bool> Delete(Guid id, Guid userId);
	}
}
