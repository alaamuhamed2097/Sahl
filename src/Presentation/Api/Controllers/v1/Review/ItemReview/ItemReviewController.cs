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
	//[Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Admin)}")]

	public class ItemReviewController : BaseController
	{
		private readonly IItemReviewService _reviewItemService;
		
		public ItemReviewController(IItemReviewService reviewService, ILogger<CartController> logger)
		{
			_reviewItemService = reviewService;
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
            var result = await _reviewItemService.GetPageAsync(criteria);

            return Ok(new ResponseModel<PagedResult<ItemReviewResponseDto>>
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
        public async Task<IActionResult> Get(Guid reviewId)
        {
            var review = await _reviewItemService.FindReviewByIdAsync(reviewId);


            if (review == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<ItemReviewResponseDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = review
            });
        }

        /// <summary>
        /// Get Item review statistics (average rating and count)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("Item-review-summery/{ItemId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemReviewSummery(Guid ItemId)
        {
            var stats = await _reviewItemService.GetItemReviewSummeryAsync(ItemId);

            return Ok(new ResponseModel<ResponseItemReviewSummeryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = stats
            });
        }

        /// <summary>
        /// Submit a new review for a Item
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPost("Add")]
		[Authorize(Roles = nameof(UserRole.Customer))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddReview([FromBody] ItemReviewDto reviewDto)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
            }

			var result = await _reviewItemService.CreateReviewAsync(reviewDto, GuidUserId);
			if (result == null)
			{
				return Ok(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.SaveFailed
				});
            }

            return Ok(new ResponseModel<ItemReviewResponseDto>
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
		[HttpPut("Update")]
		[Authorize(Roles = $"{nameof(UserRole.Customer)}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> UpdateReview([FromBody] ItemReviewDto reviewDto)
		{
			if(!ModelState.IsValid)
				{ return BadRequest(ModelState); }

			var result = await _reviewItemService.UpdateReviewAsync(reviewDto, GuidUserId);
			return Ok(new ResponseModel<ItemReviewResponseDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = result
			});
		}

        /// <summary>
        /// change a review status (Admin only)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPatch("changeStatus/{reviewId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeReviewStatus(Guid reviewId, [FromBody] ReviewStatus newStatus)
        {
            if(!ModelState.IsValid)
			{ 
				return BadRequest(ModelState); 
			}

            var result = await _reviewItemService.ChangeReviewStatus(reviewId, newStatus, UserId);
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
		/// Delete a review
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpDelete("delete")]
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
    }
}
