using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.PromoCode;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Merchandising.PromoCode;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Controller for managing vendor promo code participation requests
    /// Vendors use this to request participation in public promo codes
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VendorPromoCodeParticipationController : BaseController
    {
        private readonly IVendorPromoCodeParticipationService _participationService;

        public VendorPromoCodeParticipationController(
            IVendorPromoCodeParticipationService participationService)
        {
            _participationService = participationService;
        }

        /// <summary>
        /// Submits a participation request for a vendor to join a public promo code
        /// </summary>
        /// <param name="request">The participation request details</param>
        /// <returns>The created request details</returns>
        /// <response code="200">Request submitted successfully</response>
        /// <response code="400">Invalid input or vendor not found</response>
        /// <response code="401">Unauthorized - vendor must be authenticated</response>
        /// <response code="404">Promo code not found</response>
        /// <response code="409">Vendor already has a pending or approved request for this promo code</response>
        [HttpPost("submit")]
        [Authorize(Roles = nameof(UserRole.Vendor))]
        [ProducesResponseType(typeof(ResponseModel<VendorPromoCodeParticipationRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SubmitParticipationRequestAsync([FromBody] CreateVendorPromoCodeParticipationRequestDto request)
        {
            if (request == null)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            if (request.PromoCodeId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Promo code ID is required"
                });

            try
            {
                // Call service to submit request
                var serviceResponse = await _participationService.SubmitParticipationRequestAsync(
                    GuidUserId,
                    request,
                    GuidUserId);

                bool success = serviceResponse.Success;
                string message = serviceResponse.Message;
                VendorPromoCodeParticipationRequestDto? data = serviceResponse.Data;

                if (!success)
                {
                    if (message.Contains("already has a pending or approved request"))
                        return Conflict(new ResponseModel<string>
                        {
                            Success = false,
                            Message = message
                        });
                    else if (message.Contains("not found"))
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = message
                        });
                    else
                        return BadRequest(new ResponseModel<string>
                        {
                            Success = false,
                            Message = message
                        });
                }

                return Ok(new ResponseModel<VendorPromoCodeParticipationRequestDto>
                {
                    Success = true,
                    Message = "Participation request submitted successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Gets all promo code participation requests for the current vendor
        /// </summary>
        /// <param name="criteria">Pagination and search criteria</param>
        /// <returns>Paginated list of participation requests</returns>
        /// <response code="200">Requests retrieved successfully</response>
        /// <response code="400">Vendor not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("list")]
        [Authorize(Roles = nameof(UserRole.Vendor))]
        [ProducesResponseType(typeof(ResponseModel<AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVendorParticipationRequestsAsync([FromBody] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Call service to get requests
                var serviceResponse = await _participationService.GetVendorParticipationRequestsAsync(
                    GuidUserId,
                    criteria);

                bool success = serviceResponse.Success;
                AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>? data = serviceResponse.Data;

                if (!success || data == null)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to retrieve participation requests"
                    });

                return Ok(new ResponseModel<AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>>
                {
                    Success = true,
                    Message = "Data retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Gets details of a specific promo code participation request
        /// </summary>
        /// <param name="id">The request ID</param>
        /// <returns>The request details</returns>
        /// <response code="200">Request retrieved successfully</response>
        /// <response code="404">Request not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Vendor))]
        [ProducesResponseType(typeof(ResponseModel<VendorPromoCodeParticipationRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetParticipationRequestAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            try
            {
                // Call service to get request
                var serviceResponse = await _participationService.GetParticipationRequestAsync(id, GuidUserId);

                bool success = serviceResponse.Success;
                string message = serviceResponse.Message;
                VendorPromoCodeParticipationRequestDto? data = serviceResponse.Data;

                if (!success || data == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = message
                    });

                return Ok(new ResponseModel<VendorPromoCodeParticipationRequestDto>
                {
                    Success = true,
                    Message = "Data retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Cancels a pending promo code participation request
        /// </summary>
        /// <param name="id">The request ID to cancel</param>
        /// <returns>Success message</returns>
        /// <response code="200">Request cancelled successfully</response>
        /// <response code="400">Request cannot be cancelled (not pending)</response>
        /// <response code="404">Request not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Vendor))]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelParticipationRequestAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            try
            {
                // Call service to cancel request
                var serviceResponse = await _participationService.CancelParticipationRequestAsync(id, GuidUserId, GuidUserId);

                bool success = serviceResponse.Success;
                string message = serviceResponse.Message;

                if (!success)
                {
                    if (message.Contains("not found"))
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = message
                        });
                    else
                        return BadRequest(new ResponseModel<string>
                        {
                            Success = false,
                            Message = message
                        });
                }

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Request cancelled successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
    }
}
