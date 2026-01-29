using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;

namespace Dashboard.Services.HomePageSlider
{
	public class HomePageSliderService : IHomePageSliderService
	{
		private readonly IApiService _apiService;

		public HomePageSliderService(IApiService apiService)
		{
			_apiService = apiService;
		}

		/// <summary>
		/// Get all main banners.
		/// </summary>
		public async Task<ResponseModel<IEnumerable<HomePageSliderDto>>> GetAllAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<HomePageSliderDto>>($"{ApiEndpoints.HomePageSlider.Get}");
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<IEnumerable<HomePageSliderDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get main banner by ID.
		/// </summary>
		public async Task<ResponseModel<HomePageSliderDto>> GetByIdAsync(Guid id)
		{
			try
			{
				return await _apiService.GetAsync<HomePageSliderDto>($"{ApiEndpoints.HomePageSlider.GetById}/{id}");
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<HomePageSliderDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Save or update a main banner.
		/// </summary>
		public async Task<ResponseModel<bool>> SaveAsync(HomePageSliderDto mainBannerDto)
		{
			if (mainBannerDto == null) throw new ArgumentNullException(nameof(mainBannerDto));

			try
			{
				return await _apiService.PostAsync<HomePageSliderDto, bool>($"{ApiEndpoints.HomePageSlider.Create}", mainBannerDto); 
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Delete a attribute by ID.
		/// </summary>
		public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
		{
			try
			{
				return await _apiService.DeleteAsync< bool>($"{ApiEndpoints.HomePageSlider.Delete}/ {id}");
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<bool>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				};
			}
		}
	}

}
