using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Services.Merchandising.Campaign;
using Common.Enumerations.User;
using Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using static Shared.ErrorCodes.ErrorCodes;

namespace Api.Controllers.v1.Campaign
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Vendor)}")]
	public class CampaignItemController : BaseController
	{
		private readonly ICampaignItemService _campaignItemService;
		private readonly ILogger<CampaignController> _logger;

		public CampaignItemController(
			ICampaignItemService campaignItemService,
			ILogger<CampaignController> logger)
		{
			_campaignItemService = campaignItemService;
			_logger = logger;
		}

		#region Campaign Items
		[HttpGet("{campaignId}/search")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> SearchCampaigns(Guid campaignId,[FromQuery] BaseSearchCriteriaModel searchCriteria )
		{
			var result = await _campaignItemService.SearchCampaignItemsAsync(searchCriteria, campaignId);

			if (!result.Success)
			{
				return StatusCode(result.StatusCode, result);
			}

			return Ok(result);
		}
		/// <summary>
		/// Get campaign items (public)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("{id:guid}/items")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<CampaignItemDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCampaignItems(Guid id)
		{
			var items = await _campaignItemService.GetCampaignItemsAsync(id);

			return Ok(new ResponseModel<IEnumerable<CampaignItemDto>>
			{
				Success = true,
				Data = items,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		/// <summary>
		/// Add item to campaign (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("create")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<CampaignItemDto>), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddItemToCampaign([FromBody] AddCampaignItemDto dto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			if (!ModelState.IsValid)
				return BadRequest(new ResponseModel<object>
				{
					Success = false,
					Message = "Validation failed",
					Errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList()
				});

			var item = await _campaignItemService.AddItemToCampaignAsync(dto, GuidUserId);

			return CreatedAtAction(
				nameof(GetCampaignItems),
				new { id = dto.CampaignId },
				new ResponseModel<CampaignItemDto>
				{
					Success = true,
					Data = item,
					Message = NotifiAndAlertsResources.SavedSuccessfully
				});
		}

		/// <summary>
		/// Remove item from campaign (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("items/remove")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> RemoveItemFromCampaign(Guid itemId)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _campaignItemService.RemoveItemFromCampaignAsync(itemId, GuidUserId);

			if (!result)
			{
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});
			}

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Data = result,
				Message = NotifiAndAlertsResources.DeletedSuccessfully
			});
		}

		#endregion

		//#region Campaign whith vendor
		//[HttpGet("{campaignVendorId}/search")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status200OK)]
		//[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> SearchCampaignsItemForVendor(Guid campaignVendorId, [FromQuery] BaseSearchCriteriaModel searchCriteria)
		//{

		//	var result = await _campaignItemService.SearchCampaignItemsForVendorAsync(searchCriteria, campaignVendorId,GuidUserId);

		//	if (!result.Success)
		//	{
		//		return StatusCode(result.StatusCode, result);
		//	}

		//	return Ok(result);
		//}
		///// <summary>
		///// Get campaign items (public)
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// </remarks>
		//[HttpGet("{id:guid}/items")]
		//[AllowAnonymous]
		//[ProducesResponseType(typeof(ResponseModel<IEnumerable<CampaignItemDto>>), StatusCodes.Status200OK)]
		//public async Task<IActionResult> GetCampaignItems(Guid id)
		//{
		//	var items = await _campaignItemService.GetCampaignItemsAsync(id);

		//	return Ok(new ResponseModel<IEnumerable<CampaignItemDto>>
		//	{
		//		Success = true,
		//		Data = items,
		//		Message = NotifiAndAlertsResources.DataRetrieved
		//	});
		//}

		///// <summary>
		///// Add item to campaign (admin only)
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Admin role.
		///// Requires Authentication.
		///// </remarks>
		//[HttpPost("create")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(typeof(ResponseModel<CampaignItemDto>), StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//public async Task<IActionResult> AddItemToCampaign([FromBody] AddCampaignItemDto dto)
		//{
		//	if (string.IsNullOrEmpty(UserId))
		//		return Unauthorized(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.UnauthorizedAccess
		//		});

		//	if (!ModelState.IsValid)
		//		return BadRequest(new ResponseModel<object>
		//		{
		//			Success = false,
		//			Message = "Validation failed",
		//			Errors = ModelState.Values
		//				.SelectMany(v => v.Errors)
		//				.Select(e => e.ErrorMessage)
		//				.ToList()
		//		});

		//	var item = await _campaignItemService.AddItemToCampaignAsync(dto, GuidUserId);

		//	return CreatedAtAction(
		//		nameof(GetCampaignItems),
		//		new { id = dto.CampaignId },
		//		new ResponseModel<CampaignItemDto>
		//		{
		//			Success = true,
		//			Data = item,
		//			Message = NotifiAndAlertsResources.SavedSuccessfully
		//		});
		//}

		///// <summary>
		///// Remove item from campaign (admin only)
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Admin role.
		///// Requires Authentication.
		///// </remarks>
		//[HttpPost("items/remove")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//public async Task<IActionResult> RemoveItemFromCampaign(Guid itemId)
		//{
		//	if (string.IsNullOrEmpty(UserId))
		//		return Unauthorized(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.UnauthorizedAccess
		//		});

		//	var result = await _campaignItemService.RemoveItemFromCampaignAsync(itemId, GuidUserId);

		//	if (!result)
		//	{
		//		return NotFound(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.NoDataFound
		//		});
		//	}

		//	return Ok(new ResponseModel<bool>
		//	{
		//		Success = true,
		//		Data = result,
		//		Message = NotifiAndAlertsResources.DeletedSuccessfully
		//	});
		//}

		//#endregion


	}


}
