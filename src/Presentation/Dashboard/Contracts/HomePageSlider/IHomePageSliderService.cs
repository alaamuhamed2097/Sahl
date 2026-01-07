using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;

namespace Dashboard.Contracts.HomePageSlider
{
	public interface IHomePageSliderService
	{
		Task<ResponseModel<bool>> DeleteAsync(Guid id);
		Task<ResponseModel<IEnumerable<HomePageSliderDto>>> GetAllAsync();
		Task<ResponseModel<HomePageSliderDto>> GetByIdAsync(Guid id);
		Task<ResponseModel<bool>> SaveAsync(HomePageSliderDto mainBannerDto);
	}
}
