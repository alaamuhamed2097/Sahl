using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.Campaign;
using Common.Enumerations.User;
using Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Campaign
{
	/// <summary>
	/// Campaign and Flash Sale management
	/// </summary>
	/// <remarks>
	/// API Version: 1.0+
	/// </remarks>
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class CampaignController : BaseController
	{
		private readonly ICampaignService _campaignService;
		private readonly ILogger<CampaignController> _logger;

		public CampaignController(
			ICampaignService campaignService,
			ILogger<CampaignController> logger)
		{
			_campaignService = campaignService;
			_logger = logger;
		}

		#region Campaign Queries
		/// <summary>
		/// Get all campaigns (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllCampaigns()
		{
			var campaigns = await _campaignService.GetAllCampaignsAsync();

			return Ok(new ResponseModel<IEnumerable<CampaignDto>>
			{
				Success = true,
				Data = campaigns,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		/// <summary>
		/// Get active campaigns (public)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("active")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetActiveCampaigns()
		{
			var campaigns = await _campaignService.GetActiveCampaignsAsync();

			return Ok(new ResponseModel<IEnumerable<CampaignDto>>
			{
				Success = true,
				Data = campaigns,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		/// <summary>
		/// Get active flash sales (public)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("flash-sales/active")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetActiveFlashSales()
		{
			var flashSales = await _campaignService.GetActiveFlashSalesAsync();

			return Ok(new ResponseModel<IEnumerable<CampaignDto>>
			{
				Success = true,
				Data = flashSales,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		/// <summary>
		/// Search campaigns with pagination (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("search")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> SearchCampaigns([FromQuery] CampaignSearchCriteriaModel searchCriteria)
		{
			var result = await _campaignService.SearchCampaignsAsync(searchCriteria);

			if (!result.Success)
			{
				return StatusCode(result.StatusCode, result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Get campaign by ID (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("{id:guid}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCampaignById(Guid id)
		{
			var campaign = await _campaignService.GetCampaignByIdAsync(id);

			if (campaign == null)
			{
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});
			}

			return Ok(new ResponseModel<CampaignDto>
			{
				Success = true,
				Data = campaign,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		#endregion

		#region Campaign Management (Admin)

		/// <summary>
		/// Create new campaign (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Requires Authentication.
		/// </remarks>
		[HttpPost]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			if (dto == null)
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = "Campaign data is null"
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

			var campaign = await _campaignService.CreateCampaignAsync(dto, GuidUserId);

			return CreatedAtAction(
				nameof(GetCampaignById),
				new { id = campaign.Id },
				new ResponseModel<CampaignDto>
				{
					Success = true,
					Data = campaign,
					Message = NotifiAndAlertsResources.DeletedSuccessfully
				});
		}

		/// <summary>
		/// Update campaign (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("update")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> UpdateCampaign([FromBody] UpdateCampaignDto dto)
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

			var campaign = await _campaignService.UpdateCampaignAsync(dto, GuidUserId);

			return Ok(new ResponseModel<CampaignDto>
			{
				Success = true,
				Data = campaign,
				Message = NotifiAndAlertsResources.DataRetrieved
			});
		}

		/// <summary>
		/// Delete campaign (admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Requires Authentication.
		/// </remarks>
		[HttpDelete("{id}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> DeleteCampaign(Guid id)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _campaignService.DeleteCampaignAsync(id, GuidUserId);

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

		
	}
}

//using Api.Controllers.v1.Base;
//using Asp.Versioning;
//using BL.Contracts.Service.Merchandising.Campaign;
//using Common.Filters;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs.Campaign;
//using Shared.GeneralModels;

//namespace Api.Controllers.v1.Campaign
//{
//    /// <summary>
//    /// Campaign and Flash Sale management
//    /// </summary>
//    [ApiController]
//    [ApiVersion("1.0")]
//    [Route("api/v{version:apiVersion}/campaigns")]
//    public class CampaignController : BaseController
//    {
//        private readonly ICampaignService _campaignService;

//        public CampaignController(
//            ICampaignService campaignService)
//        {
//            _campaignService = campaignService;
//        }

//        #region Campaign Queries

//        /// <summary>
//        /// Get active campaigns (public)
//        /// </summary>
//        [HttpGet("active")]
//        [AllowAnonymous]
//        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), 200)]
//        public async Task<IActionResult> GetActiveCampaigns()
//        {
//            var campaigns = await _campaignService.GetActiveCampaignsAsync();

//            return Ok(new ResponseModel<List<CampaignDto>>
//            {
//                Success = true,
//                Data = campaigns,
//                Message = "Active campaigns retrieved successfully"
//            });
//        }

//        /// <summary>
//        /// Get active flash sales (public)
//        /// </summary>
//        [HttpGet("flash-sales/active")]
//        [AllowAnonymous]
//        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), 200)]
//        public async Task<IActionResult> GetActiveFlashSales()
//        {
//            var flashSales = await _campaignService.GetActiveFlashSalesAsync();

//            return Ok(new ResponseModel<List<CampaignDto>>
//            {
//                Success = true,
//                Data = flashSales,
//                Message = "Active flash sales retrieved successfully"
//            });
//        }

//        /// <summary>
//        /// Get all campaigns (admin only)
//        /// </summary>
//        [HttpGet]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), 200)]
//        public async Task<IActionResult> GetAllCampaigns()
//        {
//            var campaigns = await _campaignService.GetAllCampaignsAsync();

//            return Ok(new ResponseModel<List<CampaignDto>>
//            {
//                Success = true,
//                Data = campaigns,
//                Message = "All campaigns retrieved successfully"
//            });
//        }

//        /// <summary>
//        /// Search campaigns with pagination (admin only)
//        /// </summary>
//        [HttpGet("search")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<PaginatedSearchResult<CampaignDto>>), 200)]
//        public async Task<IActionResult> SearchCampaigns([FromQuery] BaseSearchCriteriaModel searchCriteria)
//        {
//            var (campaigns, totalCount) = await _campaignService.SearchCampaignsAsync(searchCriteria);

//            return Ok(new ResponseModel<PaginatedSearchResult<CampaignDto>>
//            {
//                Success = true,
//                Data = new PaginatedSearchResult<CampaignDto>
//                {
//                    Items = campaigns,
//                    TotalRecords = totalCount
//                },
//                Message = "Campaigns searched successfully"
//            });
//        }

//        /// <summary>
//        /// Get campaign by ID (admin only)
//        /// </summary>
//        [HttpGet("{id:guid}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 200)]
//        [ProducesResponseType(404)]
//        public async Task<IActionResult> GetCampaignById(Guid id)
//        {
//            var campaign = await _campaignService.GetCampaignByIdAsync(id);

//            if (campaign == null)
//            {
//                return NotFound(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Campaign not found"
//                });
//            }

//            return Ok(new ResponseModel<CampaignDto>
//            {
//                Success = true,
//                Data = campaign,
//                Message = "Campaign retrieved successfully"
//            });
//        }

//        #endregion

//        #region Campaign Management (Admin)

//        /// <summary>
//        /// Create new campaign (admin only)
//        /// </summary>
//        [HttpPost]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 201)]
//        [ProducesResponseType(400)]
//        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Errors = ModelState.Values
//                        .SelectMany(v => v.Errors)
//                        .Select(e => e.ErrorMessage)
//                        .ToList()
//                });
//            }

//            var campaign = await _campaignService.CreateCampaignAsync(dto, GuidUserId);

//            return CreatedAtAction(
//                nameof(GetCampaignById),
//                new { id = campaign.Id },
//                new ResponseModel<CampaignDto>
//                {
//                    Success = true,
//                    Data = campaign,
//                    Message = "Campaign created successfully"
//                });
//        }

//        /// <summary>
//        /// Update campaign (admin only)
//        /// </summary>
//        [HttpPut("{id:guid}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 200)]
//        [ProducesResponseType(404)]
//        [ProducesResponseType(400)]
//        public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] UpdateCampaignDto dto)
//        {
//            if (id != dto.Id)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Campaign ID mismatch"
//                });
//            }

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Errors = ModelState.Values
//                        .SelectMany(v => v.Errors)
//                        .Select(e => e.ErrorMessage)
//                        .ToList()
//                });
//            }

//            var campaign = await _campaignService.UpdateCampaignAsync(dto, GuidUserId);

//            return Ok(new ResponseModel<CampaignDto>
//            {
//                Success = true,
//                Data = campaign,
//                Message = "Campaign updated successfully"
//            });
//        }

//        /// <summary>
//        /// Delete campaign (admin only)
//        /// </summary>
//        [HttpDelete("{id:guid}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<object>), 200)]
//        [ProducesResponseType(404)]
//        public async Task<IActionResult> DeleteCampaign(Guid id)
//        {
//            var result = await _campaignService.DeleteCampaignAsync(id, GuidUserId);

//            if (!result)
//            {
//                return NotFound(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Campaign not found"
//                });
//            }

//            return Ok(new ResponseModel<object>
//            {
//                Success = true,
//                Message = "Campaign deleted successfully"
//            });
//        }

//        #endregion

//        #region Campaign Items

//        /// <summary>
//        /// Get campaign items (public)
//        /// </summary>
//        [HttpGet("{id:guid}/items")]
//        [AllowAnonymous]
//        [ProducesResponseType(typeof(ResponseModel<List<CampaignItemDto>>), 200)]
//        public async Task<IActionResult> GetCampaignItems(Guid id)
//        {
//            var items = await _campaignService.GetCampaignItemsAsync(id);

//            return Ok(new ResponseModel<List<CampaignItemDto>>
//            {
//                Success = true,
//                Data = items,
//                Message = "Campaign items retrieved successfully"
//            });
//        }

//        /// <summary>
//        /// Add item to campaign (admin only)
//        /// </summary>
//        [HttpPost("{id:guid}/items")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<CampaignItemDto>), 201)]
//        [ProducesResponseType(400)]
//        public async Task<IActionResult> AddItemToCampaign(Guid id, [FromBody] AddCampaignItemDto dto)
//        {
//            if (id != dto.CampaignId)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Campaign ID mismatch"
//                });
//            }

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Errors = ModelState.Values
//                        .SelectMany(v => v.Errors)
//                        .Select(e => e.ErrorMessage)
//                        .ToList()
//                });
//            }

//            var item = await _campaignService.AddItemToCampaignAsync(dto, GuidUserId);

//            return CreatedAtAction(
//                nameof(GetCampaignItems),
//                new { id = dto.CampaignId },
//                new ResponseModel<CampaignItemDto>
//                {
//                    Success = true,
//                    Data = item,
//                    Message = "Item added to campaign successfully"
//                });
//        }

//        /// <summary>
//        /// Remove item from campaign (admin only)
//        /// </summary>
//        [HttpDelete("{campaignId:guid}/items/{itemId:guid}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(typeof(ResponseModel<object>), 200)]
//        [ProducesResponseType(404)]
//        public async Task<IActionResult> RemoveItemFromCampaign(Guid campaignId, Guid itemId)
//        {
//            var result = await _campaignService.RemoveItemFromCampaignAsync(itemId, GuidUserId);

//            if (!result)
//            {
//                return NotFound(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Campaign item not found"
//                });
//            }

//            return Ok(new ResponseModel<object>
//            {
//                Success = true,
//                Message = "Item removed from campaign successfully"
//            });
//        }

//        #endregion
//    }
//}