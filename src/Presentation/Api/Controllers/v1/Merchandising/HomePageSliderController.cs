using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.HomePageSlider;

using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Merchandising
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class HomePageSliderController : BaseController
	{
		private readonly IHomePageSliderService _homePageSliderService;

		public HomePageSliderController(IHomePageSliderService homePageSliderService)
		{
			_homePageSliderService = homePageSliderService;
		}

		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
		{
			

				var result = await _homePageSliderService.GetPage(criteria);

				if (result == null || !result.Items.Any())
				{
					return Ok(new ResponseModel<PagedResult<HomePageSliderDto>>
					{
						Message = NotifiAndAlertsResources.NoDataFound,
						Data = result
					});
				}

				return Ok(new ResponseModel<PagedResult<HomePageSliderDto>>
				{
					Message = NotifiAndAlertsResources.DataRetrieved,
					Data = result
				});
			
		}
		/// <summary>
		/// Get slider by ID with full details
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("{sliderId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
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

		/// <summary>
		/// Get all sliders (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("all")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllSliders()
		{
			var sliders = await _homePageSliderService.GetAllAsync();

			if (sliders == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = sliders
			});
		}

		
		

		/// <summary>
		/// Create new slider
		/// </summary>
		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateSlider([FromBody] HomePageSliderDto dto)
		{


			 var slider = await _homePageSliderService.CreateAsync(dto, GuidUserId);

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
				
			});
		}

		/// <summary>
		/// Update existing slider
		/// </summary>
		[HttpPost("update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateSlider([FromBody]  HomePageSliderDto dto)
		{
			var isSlider = await _homePageSliderService.FindByIdAsync(dto.Id);

			if (isSlider == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			var slider = await _homePageSliderService.UpdateAsync(dto, GuidUserId);

			return Ok(new ResponseModel<HomePageSliderDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,

			});
		}

		/// <summary>
		/// Delete slider
		/// </summary>
		[HttpDelete("{sliderId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteSlider(Guid sliderId)
		{
			var isSlider = await _homePageSliderService.FindByIdAsync(sliderId);
			if (isSlider == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			var result = await _homePageSliderService.DeleteAsync(sliderId, GuidUserId);
			return Ok(result);
		}

		// ==================== BATCH OPERATIONS ====================

		/// <summary>
		/// Get multiple sliders by IDs
		/// </summary>
		//[HttpPost("batch/get")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> GetSlidersByIds([FromBody] BatchIdsRequest request)
		//{
		//	if (request?.Ids == null || !request.Ids.Any())
		//		return BadRequest(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = ValidationResources.InvalidData
		//		});

		//	var sliders = await _homePageSliderService.GetByIdsAsync(request.Ids);

		//	return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DataRetrieved,
		//		Data = sliders
		//	});
		//}

		/// <summary>
		/// Activate multiple sliders
		/// </summary>
		//[HttpPost("batch/activate")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> ActivateSliders([FromBody] BatchIdsRequest request)
		//{
		//	if (request?.Ids == null || !request.Ids.Any())
		//		return BadRequest(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = ValidationResources.InvalidData
		//		});

		//	var result = await _homePageSliderService.ActivateBatchAsync(request.Ids);
		//	return Ok(result);
		//}

		/// <summary>
		/// Deactivate multiple sliders
		/// </summary>
		//[HttpPost("batch/deactivate")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> DeactivateSliders([FromBody] BatchIdsRequest request)
		//{
		//	if (request?.Ids == null || !request.Ids.Any())
		//		return BadRequest(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = ValidationResources.InvalidData
		//		});

		//	var result = await _homePageSliderService.DeactivateBatchAsync(request.Ids);
		//	return Ok(result);
		//}

		/// <summary>
		/// Delete multiple sliders
		/// </summary>
		//[HttpPost("batch/delete")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> DeleteSliders([FromBody] BatchIdsRequest request)
		//{
		//	if (request?.Ids == null || !request.Ids.Any())
		//		return BadRequest(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = ValidationResources.InvalidData
		//		});

		//	var result = await _homePageSliderService.DeleteBatchAsync(request.Ids);
		//	return Ok(result);
		//}

		/// <summary>
		/// Update display order for multiple sliders
		/// </summary>
		//[HttpPost("batch/reorder")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> ReorderSliders([FromBody] BatchReorderRequest request)
		//{
		//	if (request?.Items == null || !request.Items.Any())
		//		return BadRequest(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = ValidationResources.InvalidData
		//		});

		//	var result = await _homePageSliderService.UpdateDisplayOrderBatchAsync(request.Items);
		//	return Ok(result);
		//}
	}

	// Request Models
	//public class BatchIdsRequest
	//{
	//	public List<Guid> Ids { get; set; } = new();
	//}

	//public class BatchReorderRequest
	//{
	//	public List<ReorderItem> Items { get; set; } = new();
	//}

	//public class ReorderItem
	//{
	//	public Guid Id { get; set; }
	//	public int DisplayOrder { get; set; }
	//}
}
