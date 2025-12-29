using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.HomeSlider;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.HomeSlider
{
	public interface IHomePageSliderService : IBaseService<TbHomePageSlider, HomePageSliderDto>
	{
		/// <summary>
		/// Get paginated list of home page sliders
		/// </summary>
		Task<PagedResult<HomePageSliderDto>> GetPage(BaseSearchCriteriaModel criteriaModel);

		/// <summary>
		/// Get all active sliders within current date range
		/// </summary>
		Task<IEnumerable<HomePageSliderDto>> GetActiveSliders();
		//Task<bool> Save(HomePageSliderDto dto, Guid userId);
		//Task<bool> Delete(Guid id, Guid userId);
	}
}
