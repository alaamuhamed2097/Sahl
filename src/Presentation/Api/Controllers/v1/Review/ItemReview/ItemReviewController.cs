using Api.Controllers.v1.Base;
using Api.Controllers.v1.Order;
using Asp.Versioning;
using BL.Contracts.Service.Review;
using Common.Enumerations.Review;
using Common.Enumerations.User;
using DAL.Exceptions;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Security.Claims;

using static Shared.ErrorCodes.ErrorCodes;

namespace Api.Controllers.v1.Review.ItemReview
{

	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize]
	//[Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Admin)}")]

	public class ItemReviewController : BaseController
	{
		private readonly IItemReviewService _reviewItemService;
		
		public ItemReviewController(IItemReviewService reviewService, ILogger<CartController> logger)
		{
			_reviewItemService = reviewService;
		}

		
		/// <summary>
		/// Submit a new review for a Item
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("submit")]
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> SubmitReview([FromBody] ItemReviewDto reviewDto)
		{
			//if (string.IsNullOrEmpty(UserId))
			//	return Unauthorized(new ResponseModel<string>
			//	{
			//		Success = false,
			//		Message = NotifiAndAlertsResources.UnauthorizedAccess
			//	});

			//reviewDto.CustomerID = Guid.Parse(UserId);
			var result = await _reviewItemService.SubmitReviewAsync(reviewDto, GuidUserId);

			return Ok(new ResponseModel<ResponseItemReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Edit an existing review
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("update")]
		[Authorize(Roles = $"{nameof(UserRole.Customer)}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> updateReview( [FromBody] ItemReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewItemService.updateReviewAsync( reviewDto, GuidUserId);

			return Ok(new ResponseModel<ResponseItemReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Get review by ID with full details
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("{reviewId}")]
		[Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Admin)}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetReviewById(Guid reviewId)
		{
			var review = await _reviewItemService.GetReviewByIdAsync(reviewId);
			

			if (review == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<ResponseItemReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = review
			});
		}


		/// <summary>
		/// Delete a review
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("delete")]
		[Authorize(Roles = $"{nameof(UserRole.Customer)}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteReview([FromBody] ItemReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewItemService.DeleteReviewAsync(reviewDto.Id, GuidUserId);

			if (!result)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DeletedSuccessfully,
				Data = result
			});
		}

		/// <summary>
		/// Get all approved reviews for a Item
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("reviews-by-Item/{ItemId}")]
		[Authorize(Roles = $"{nameof(UserRole.Admin)}")]

		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewsByItem(Guid ItemId)
		{
			var reviews = await _reviewItemService.GetReviewsByItemIdAsync(ItemId);

			return Ok(new ResponseModel<IEnumerable<ResponseItemReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviews
			});
		}

		/// <summary>
		/// Get paginated reviews with filters
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("search")]
		[Authorize(Roles = $"{nameof(UserRole.Admin)}")]

		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Search([FromQuery] ItemReviewSearchCriteriaModel criteria)
		{
			var result = await _reviewItemService.GetPaginatedReviewsAsync(criteria);

			return Ok(new ResponseModel<PagedResult<ResponseItemReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}
		/// <summary>
		/// Get Item review statistics (average rating and count)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("Item-review-stats/{ItemId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetItemReviewStats(Guid ItemId)
		{
			var stats = await _reviewItemService.GetItemReviewStatsAsync(ItemId);

			return Ok(new ResponseModel<ResponseItemReviewStatsDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = stats
			});
		}

		/// <summary>
		/// Get pending reviews for moderation (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("pending")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetPendingReviews()
		{
			var reviews = await _reviewItemService.GetPendingReviewsAsync();

			return Ok(new ResponseModel<IEnumerable<ResponseItemReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviews
			});
		}


		/// <summary>
		/// Approve a review (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPost("approve")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ApproveReview([FromBody] ItemReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewItemService.ApproveReviewAsync(reviewDto.Id, GuidUserId);

			if (!result)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Reject a review (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPost("reject")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> RejectReview([FromBody] ItemReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewItemService.RejectReviewAsync(reviewDto.Id, GuidUserId);

			if (!result)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}
	}
}
