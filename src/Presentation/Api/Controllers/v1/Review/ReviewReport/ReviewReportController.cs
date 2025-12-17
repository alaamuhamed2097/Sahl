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

namespace Api.Controllers.v1.Review.ReviewReport
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class ReviewReportController : BaseController
	{
		private readonly IReviewReportService _reportService;

		public ReviewReportController(IReviewReportService reportService)
		{
			_reportService = reportService;
		}


		/// <summary>
		/// Report a review
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("Submit")]
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> ReportReview( [FromBody] ReviewReportDto reportDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			
			reportDto.CustomerID = Guid.Parse(UserId);

			var result = await _reportService.SubmitReportAsync(reportDto);

			return Ok(new ResponseModel<string>
			{
				Success = true,
				Message = NotifiAndAlertsResources.SavedSuccessfully,
				
			});
		}

		/// <summary>
		/// Get report by ID
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("{reportId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetReportById(Guid reportId)
		{
			var report = await _reportService.GetReportByIdAsync(reportId);

			if (report == null)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<ReviewReportDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = report
			});
		}

		/// <summary>
		/// Get all reports with optional status filter (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("Search")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Search(ReviewReportSearchCriteriaModel criteriaModel)
		{
			var result = await _reportService.GetPaginatedReviewReportsAsync(criteriaModel);

			return Ok(new ResponseModel<PaginatedDataModel<ReviewReportDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

		/// <summary>
		/// Get all reports for a specific review (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("reports-by-offer/{offerReviewId}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetReportsByReview(Guid offerReviewId)
		{
			var reports = await _reportService.GetReportsByReviewIdAsync(offerReviewId);

			return Ok(new ResponseModel<IEnumerable<ReviewReportDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = reports
			});
		}

		/// <summary>
		/// Resolve a report (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPost("resolve")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ResolveReport([FromBody] ReviewReportDto reportDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reportService.ResolveReportAsync(reportDto.Id, GuidUserId);

			if (!result.Success)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<string>
			{
				Success = true,
				Message = NotifiAndAlertsResources.ItemUpdated,
				
			});
		}

		/// <summary>
		/// Mark a review as flagged (Admin only)
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPost("MarkAsFlagged")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> MarkAsFlagged([FromBody] OfferReviewDto reviewDto)
		{
			if (string.IsNullOrEmpty(UserId) || reviewDto == null)
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _reportService.MarkReviewAsFlaggedAsync(reviewDto.Id, UserId);

			if (!result)
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.ItemUpdated,
				Data = result
			});
		}
	}
}
