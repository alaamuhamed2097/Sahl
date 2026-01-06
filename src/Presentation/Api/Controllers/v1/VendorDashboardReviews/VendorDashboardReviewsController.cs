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

namespace Api.Controllers.v1.VendorDashboardReviews
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = $"{nameof(UserRole.Vendor)}")]
	public class VendorDashboardReviewsController : BaseController
	{
		private readonly IVendorReviewService _vendorReviewService;
		private readonly ILogger<VendorDashboardReviewsController> _logger;

		public VendorDashboardReviewsController(
			IVendorReviewService vendorReviewService,
			ILogger<VendorDashboardReviewsController> logger)
		{
			_vendorReviewService = vendorReviewService;
			_logger = logger;
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
		public async Task<IActionResult> GetReviewsByVendor(Guid vendorId)
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
		public async Task<IActionResult> Search([FromQuery] VendorReviewSearchCriteriaModel criteria)
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
		[HttpGet("vendor-review-stats/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVendorReviewStats(Guid vendorId)
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
		[HttpGet("vendor/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetVendorReviews(
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
		[HttpGet("average-rating/{vendorId}")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAverageRating(Guid vendorId)
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
		public async Task<IActionResult> GetReviewCount(Guid vendorId)
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