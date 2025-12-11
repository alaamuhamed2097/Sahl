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

namespace Api.Controllers.v1.Review.OfferReview
{

	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = nameof(UserRole.Customer))]
	public class OfferReviewController : BaseController
	{
		private readonly IOfferReviewService _reviewService;
		public OfferReviewController(IOfferReviewService reviewService, ILogger<CartController> logger)
		{
			_reviewService = reviewService;
		}

		/// <summary>
		/// Get review by ID with full details
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("{reviewId}")]
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetReviewById(Guid reviewId)
		{
			var review = await _reviewService.GetReviewByIdAsync(reviewId);

			if (review == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<OfferReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = review
			});
		}

		/// <summary>
		/// Submit a new review for a Offer
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
		public async Task<IActionResult> SubmitReview([FromBody] OfferReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			reviewDto.CustomerID = Guid.Parse(UserId);
			var result = await _reviewService.SubmitReviewAsync(reviewDto);

			return Ok(new ResponseModel<OfferReviewDto>
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
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> updateReview( [FromBody] OfferReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.updateReviewAsync( reviewDto, GuidUserId);

			return Ok(new ResponseModel<OfferReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
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
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteReview([FromBody] Guid reviewId)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.DeleteReviewAsync(reviewId, GuidUserId);

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
		/// Get all approved reviews for a Offer
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("reviews-by-offer/{OfferId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewsByOffer(Guid OfferId)
		{
			var reviews = await _reviewService.GetReviewsByOfferIdAsync(OfferId);

			return Ok(new ResponseModel<IEnumerable<OfferReviewDto>>
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Search([FromQuery] OfferReviewSearchCriteriaModel criteria)
		{
			var result = await _reviewService.GetPaginatedReviewsAsync(criteria);

			return Ok(new ResponseModel<PaginatedDataModel<OfferReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}
		/// <summary>
		/// Get Offer review statistics (average rating and count)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("Offer-review-stats/{OfferId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetOfferReviewStats(Guid OfferId)
		{
			var stats = await _reviewService.GetOfferReviewStatsAsync(OfferId);

			return Ok(new ResponseModel<OfferReviewStatsDto>
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
		public async Task<IActionResult> GetPendingReviews()
		{
			var reviews = await _reviewService.GetPendingReviewsAsync();

			return Ok(new ResponseModel<IEnumerable<OfferReviewDto>>
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
		[HttpGet("approve/{reviewId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ApproveReview(Guid reviewId)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.ApproveReviewAsync(reviewId, GuidUserId);

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
		[HttpGet("reject/{reviewId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> RejectReview(Guid reviewId)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.RejectReviewAsync(reviewId, GuidUserId);

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
