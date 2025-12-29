using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.HomeSlider;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.HomeSlider
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	//[Authorize(Roles = nameof(UserRole.Admin))]
	public class HomePageSliderController : BaseController
	{
		private readonly IHomePageSliderService _homePageSliderService;

		public HomePageSliderController(IHomePageSliderService homePageSliderService, ILogger<HomePageSliderController> logger)
		{
			_homePageSliderService = homePageSliderService;
		}
		/// <summary>
		/// Get all active sliders (within date range)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("active-sliders")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetActiveSliders()
		{
			var sliders = await _homePageSliderService.GetActiveSliders();

			return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = sliders
			});
		}
		/// <summary>
		/// Get slider by ID with full details
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("{sliderId}")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSliderById(Guid sliderId)
		{
			var slider = await _homePageSliderService.FindByIdAsync(sliderId);

			if (slider == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<HomePageSliderDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = slider
			});
		}

		///// <summary>
		///// Submit a new slider
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Authentication.
		///// </remarks>
		//[HttpPost("submit")]
		////[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//public async Task<IActionResult> SubmitSlider([FromBody] HomePageSliderDto sliderDto)
		//{
		//	var result = await _homePageSliderService.SaveAsync(sliderDto, GuidUserId);

		//	return Ok(new ResponseModel<bool>
		//	{
		//		Success = result.IsSuccess,
		//		Message = result.IsSuccess ? NotifiAndAlertsResources.SavedSuccessfully : result.Message,
		//		Data = result.IsSuccess
		//	});
		//}

		/// <summary>
		/// Edit an existing slider
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		//[HttpPost("update")]
		////[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[ProducesResponseType(StatusCodes.Status403Forbidden)]
		//public async Task<IActionResult> UpdateSlider([FromBody] HomePageSliderDto sliderDto)
		//{
		//	if (string.IsNullOrEmpty(UserId))
		//		return Unauthorized(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.UnauthorizedAccess
		//		});

		//	var result = await _homePageSliderService.UpdateAsync(sliderDto, GuidUserId);

		//	return Ok(new ResponseModel<bool>
		//	{
		//		Success = result.IsSuccess,
		//		Message = result.IsSuccess ? NotifiAndAlertsResources.UpdatedSuccessfully : result.Message,
		//		Data = result.IsSuccess
		//	});
		//}

		/// <summary>
		/// Delete a slider
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		//[HttpPost("delete")]
		////[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//public async Task<IActionResult> DeleteSlider([FromBody] HomePageSliderDto sliderDto)
		//{
		//	if (string.IsNullOrEmpty(UserId))
		//		return Unauthorized(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.UnauthorizedAccess
		//		});

		//	var result = await _homePageSliderService.DeleteAsync(sliderDto.Id, GuidUserId);

		//	if (!result)
		//		return NotFound(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.NoDataFound
		//		});

		//	return Ok(new ResponseModel<bool>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DeletedSuccessfully,
		//		Data = result
		//	});
		//}

		

		/// <summary>
		/// Get paginated sliders with filters
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		//[HttpGet("search")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
		//{
		//	var result = await _homePageSliderService.GetPage(criteria);

		//	return Ok(new ResponseModel<PagedResult<HomePageSliderDto>>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DataRetrieved,
		//		Data = result
		//	});
		//}

		/// <summary>
		/// Get all sliders (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("all")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllSliders()
		{
			var sliders = await _homePageSliderService.GetAllAsync();

			return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = sliders
			});
		}
	}
}