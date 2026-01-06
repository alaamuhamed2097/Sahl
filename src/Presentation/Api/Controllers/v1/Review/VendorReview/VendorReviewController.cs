using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Review;

using Common.Enumerations.Review;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Review.VendorReview
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Admin)},{nameof(UserRole.Vendor)}")]
	public class VendorReviewController : BaseController
	{
		private readonly IVendorReviewService _reviewService;
		private readonly ILogger<VendorReviewController> _logger;
		private readonly IVendorReviewService _vendorReviewService;

		public VendorReviewController(
			IVendorReviewService reviewService,
			ILogger<VendorReviewController> logger,
			IVendorReviewService vendorReviewService)
		{
			_reviewService = reviewService;
			_logger = logger;
			_vendorReviewService = vendorReviewService;
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

			return Ok(new ResponseModel<VendorReviewDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = review
			});
		}

		/// <summary>
		/// Submit a new review for a Vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("submit")]
		//[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> SubmitReview([FromBody] VendorReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});
			
			if (reviewDto == null)
				return BadRequest("ReviewDto is null");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			
			//reviewDto.CustomerId = Guid.Parse(UserId);
			var result = await _reviewService.SubmitReviewAsync(reviewDto, GuidUserId);

			return Ok(new ResponseModel<VendorReviewDto>
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
		public async Task<IActionResult> UpdateReview([FromBody] VendorReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.UpdateReviewAsync(reviewDto, GuidUserId);

			return Ok(new ResponseModel<VendorReviewDto>
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
		public async Task<IActionResult> DeleteReview([FromBody] VendorReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.DeleteReviewAsync(reviewDto.Id, GuidUserId);

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
		/// Get all approved reviews for a Vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("reviews-by-vendor/{vendorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewsByVendor(Guid vendorId)
		{
			var reviews = await _reviewService.GetReviewsByVendorIdAsync(vendorId);

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
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
		public async Task<IActionResult> Search([FromQuery] VendorReviewSearchCriteriaModel criteria)
		{
			var result = await _reviewService.GetPaginatedReviewsAsync(criteria);

			return Ok(new ResponseModel<PagedResult<VendorReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Get Vendor review statistics (average rating and count)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("vendor-review-stats/{vendorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVendorReviewStats(Guid vendorId)
		{
			var stats = await _reviewService.GetVendorReviewStatsAsync(vendorId);

			return Ok(new ResponseModel<VendorReviewStatsDto>
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

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
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
		public async Task<IActionResult> ApproveReview([FromBody] VendorReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.ApproveReviewAsync(reviewDto.Id, GuidUserId);

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
		public async Task<IActionResult> RejectReview([FromBody] VendorReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reviewService.RejectReviewAsync(reviewDto.Id, GuidUserId);

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
		/// Get all reviews for a vendor with optional status filter
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("vendor/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVendorReviews(
			Guid vendorId,
			[FromQuery] ReviewStatus? status = null)
		{
			var reviews = await _reviewService.GetVendorReviewsAsync(vendorId, status);

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviews
			});
		}

		/// <summary>
		/// Get all reviews created by a specific customer
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpGet("customer/{customerId}")]
		//[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCustomerReviews(Guid customerId)
		{
			// Verify that the customer is accessing their own reviews
			//if (GuidUserId != customerId)
			//	return Forbid();

			var reviews = await _reviewService.GetCustomerReviewsAsync(customerId);

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviews
			});
		}

		/// <summary>
		/// Get average rating for a vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("average-rating/{vendorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAverageRating(Guid vendorId)
		{
			var averageRating = await _reviewService.GetAverageRatingAsync(vendorId);

			return Ok(new ResponseModel<decimal>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = averageRating
			});
		}

		/// <summary>
		/// Get review count for a vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("review-count/{vendorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewCount(Guid vendorId)
		{
			var reviewCount = await _reviewService.GetReviewCountAsync(vendorId);

			return Ok(new ResponseModel<int>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviewCount
			});
		}


		/// <summary>
		/// Get all approved reviews for a Vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("vendorReviews-by/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewsByVendorId(Guid vendorId)
		{
			var reviews = await _vendorReviewService.GetReviewsByVendorIdAsync(vendorId);

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
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
		[HttpGet("searchVendorReviews")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> SearchVendorReviews([FromQuery] VendorReviewSearchCriteriaModel criteria)
		{
			var result = await _vendorReviewService.GetPaginatedReviewsAsync(criteria);

			return Ok(new ResponseModel<PagedResult<VendorReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Get Vendor review statistics (average rating and count)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("vendor-review-stats-by/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVendorReviewStatsByVendorId(Guid vendorId)
		{
			var stats = await _vendorReviewService.GetVendorReviewStatsAsync(vendorId);

			return Ok(new ResponseModel<VendorReviewStatsDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = stats
			});
		}

		///// <summary>
		///// Get pending reviews for moderation (Admin only)
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Admin role.
		///// </remarks>
		//[HttpGet("pending")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//public async Task<IActionResult> GetPendingReviews()
		//{
		//	var reviews = await _reviewService.GetPendingReviewsAsync();

		//	return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DataRetrieved,
		//		Data = reviews
		//	});
		//}



		/// <summary>
		/// Get all reviews for a vendor with optional status filter
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("vendor-by/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllReviewsForVendor(
			Guid vendorId,
			[FromQuery] ReviewStatus? status = null)
		{
			var reviews = await _vendorReviewService.GetVendorReviewsAsync(vendorId, status);

			return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviews
			});
		}
		/// <summary>
		/// Get only verified purchase reviews for a vendor
		/// </summary>
		/// <param name="vendorId">Vendor ID</param>
		[HttpGet("vendor/{vendorId}/verified")]
		[ProducesResponseType(typeof(IEnumerable<VendorReviewDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVerifiedReviews(
			[FromRoute] Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (vendorId == Guid.Empty)
					return BadRequest("Vendor ID cannot be empty");

				var reviews = await _vendorReviewService.GetVendorReviewsByVerificationAsync(
					vendorId,
					true,
					cancellationToken);

				return Ok(reviews);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving verified reviews");
				return StatusCode(StatusCodes.Status500InternalServerError,
					"An error occurred while retrieving verified reviews");
			}
		}
		/// <summary>
		/// Get only non-verified purchase reviews for a vendor
		/// </summary>
		/// <param name="vendorId">Vendor ID</param>
		[HttpGet("vendor/{vendorId}/non-verified")]
		[ProducesResponseType(typeof(IEnumerable<VendorReviewDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetNonVerifiedReviews(
			[FromRoute] Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (vendorId == Guid.Empty)
					return BadRequest("Vendor ID cannot be empty");

				var reviews = await _vendorReviewService.GetVendorReviewsByVerificationAsync(
					vendorId,
					false,
					cancellationToken);

				return Ok(reviews);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving non-verified reviews");
				return StatusCode(StatusCodes.Status500InternalServerError,
					"An error occurred while retrieving non-verified reviews");
			}
		}
		///// <summary>
		///// Get all reviews created by a specific customer
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Authentication.
		///// </remarks>
		//[HttpGet("customer/{customerId}")]
		////[Authorize(Roles = nameof(UserRole.Customer))]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//public async Task<IActionResult> GetCustomerReviews(Guid customerId)
		//{
		//	// Verify that the customer is accessing their own reviews
		//	//if (GuidUserId != customerId)
		//	//	return Forbid();

		//	var reviews = await _reviewService.GetCustomerReviewsAsync(customerId);

		//	return Ok(new ResponseModel<IEnumerable<VendorReviewDto>>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DataRetrieved,
		//		Data = reviews
		//	});
		//}

		/// <summary>
		/// Get average rating for a vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("average-rating-by/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAverageRatingByVendorId(Guid vendorId)
		{
			var averageRating = await _vendorReviewService.GetAverageRatingAsync(vendorId);

			return Ok(new ResponseModel<decimal>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = averageRating
			});
		}

		/// <summary>
		/// Get review count for a vendor
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("vendorReview-count/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReviewCountForVendor(Guid vendorId)
		{
			var reviewCount = await _vendorReviewService.GetReviewCountAsync(vendorId);

			return Ok(new ResponseModel<int>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reviewCount
			});
		}



	}
}