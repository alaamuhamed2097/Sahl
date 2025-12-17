using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Review;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using static Shared.ErrorCodes.ErrorCodes;

namespace Api.Controllers.v1.Review.ReviewVote
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/reviews-vote/{reviewId}")]
	[Authorize(Roles = nameof(UserRole.Customer))]
	public class ReviewVoteController : BaseController
	{
		private readonly IReviewVoteService _voteService;

		public ReviewVoteController(IReviewVoteService voteService)
		{
			_voteService = voteService;
		}

		/// <summary>
		/// Mark a review as helpful
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("helpful")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddHelpfulVote(ReviewVoteDto reviewVoteDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _voteService.AddHelpfulVoteAsync(reviewVoteDto, GuidUserId);

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Message = NotifiAndAlertsResources.SavedSuccessfully,
				Data = result
			});
		}

		///// <summary>
		///// Mark a review as not helpful
		///// </summary>
		///// <remarks>
		///// API Version: 1.0+
		///// Requires Authentication.
		///// </remarks>
		//[HttpPost("nothelpful")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//public async Task<IActionResult> AddNotHelpfulVote(Guid reviewId)
		//{
		//	if (string.IsNullOrEmpty(UserId))
		//		return Unauthorized(new ResponseModel<string>
		//		{
		//			Success = false,
		//			Message = NotifiAndAlertsResources.UnauthorizedAccess
		//		});

		//	var result = await _voteService.AddNotHelpfulVoteAsync(reviewId, UserId);

		//	return Ok(new ResponseModel<bool>
		//	{
		//		Success = true,
		//		Message = NotifiAndAlertsResources.DataSaved,
		//		Data = result
		//	});
		//}

		/// <summary>
		/// Remove your vote from a review
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Authentication.
		/// </remarks>
		[HttpPost("delete")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> RemoveVote([FromBody] ReviewVoteDto reviewVoteDto)
		{
			if (string.IsNullOrEmpty(UserId))
				return Unauthorized(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.UnauthorizedAccess
				});

			var result = await _voteService.RemoveVoteAsync(reviewVoteDto.Id, GuidUserId);

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
