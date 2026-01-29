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
	public class HomePageSliderController : BaseController
	{
		private readonly IHomePageSliderService _homePageSliderService;

		public HomePageSliderController(IHomePageSliderService homePageSliderService)
		{
			_homePageSliderService = homePageSliderService;
		}

		/// <summary>
		/// Search sliders with pagination
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("search")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
		{
			try
			{
				var result = await _homePageSliderService.GetPage(criteria);

				if (result == null || !result.Items.Any())
				{
					return Ok(new ResponseModel<PagedResult<HomePageSliderDto>>
					{
						Success = true,
						Message = NotifiAndAlertsResources.NoDataFound,
						Data = result ?? new PagedResult<HomePageSliderDto>(new List<HomePageSliderDto>(), 0)
					});
				}

				return Ok(new ResponseModel<PagedResult<HomePageSliderDto>>
				{
					Success = true,
					Message = NotifiAndAlertsResources.DataRetrieved,
					Data = result
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Get all sliders
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Public endpoint - no authentication required.
		/// </remarks>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAllSliders()
		{
			try
			{
				var sliders = await _homePageSliderService.GetAllSliders();

				if (sliders == null || !sliders.Any())
					return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
					{
						Success = true,
						Message = NotifiAndAlertsResources.NoDataFound,
						Data = new List<HomePageSliderDto>()
					});

				return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
				{
					Success = true,
					Message = NotifiAndAlertsResources.DataRetrieved,
					Data = sliders
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Get slider by ID with full details
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("{sliderId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSliderById(Guid sliderId)
		{
			try
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
			catch (Exception ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Create or update slider
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Handles both create (Id = Guid.Empty) and update operations.
		/// Automatically manages image upload, old image deletion, and display order.
		/// </remarks>
		[HttpPost("save")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SaveSlider([FromBody] HomePageSliderDto dto)
		{
			try
			{
				if (dto == null)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = ValidationResources.InvalidFormat
					});

				var result = await _homePageSliderService.Save(dto, GuidUserId);

				if (result)
				{
					return Ok(new ResponseModel<bool>
					{
						Success = true,
						Message = dto.Id == Guid.Empty
							? NotifiAndAlertsResources.SavedSuccessfully
							: NotifiAndAlertsResources.ItemUpdated,
						Data = true
					});
				}

				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.SaveFailed
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Update slider display order
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPatch("{sliderId}/display-order")]
		[HttpPut("{sliderId}/display-order")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateDisplayOrder(Guid sliderId, [FromBody] dynamic request)
		{
			int newOrder;
			if (request is int intValue)
			{
				newOrder = intValue;
			}
			else if (request is System.Text.Json.JsonElement jsonElement)
			{
				if (jsonElement.TryGetProperty("newOrder", out var newOrderProp))
					newOrder = newOrderProp.GetInt32();
				else
					newOrder = jsonElement.GetInt32();
			}
			else
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = "Invalid display order format"
				});
			}

			var slider = await _homePageSliderService.FindByIdAsync(sliderId);
			if (slider == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			var result = await _homePageSliderService.UpdateDisplayOrderAsync(sliderId, newOrder, GuidUserId);
			if (!result)
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.SaveFailed
				});

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.ItemUpdated,
				Data = true
			});
		}

		/// <summary>
		/// Delete slider
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Automatically handles image file deletion and display order adjustment.
		/// </remarks>
		[HttpDelete("{sliderId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteSlider(Guid sliderId)
		{
			try
			{
				var isSlider = await _homePageSliderService.FindByIdAsync(sliderId);
				if (isSlider == null)
					return NotFound(new ResponseModel<string>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					});

				var result = await _homePageSliderService.Delete(sliderId, GuidUserId);

				if (result)
				{
					return Ok(new ResponseModel<bool>
					{
						Success = true,
						Message = NotifiAndAlertsResources.DeletedSuccessfully,
						Data = true
					});
				}

				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}
	}
}