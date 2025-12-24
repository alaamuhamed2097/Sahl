using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Homepage Controller - Public facing API
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/homepage")]
    public class HomepageController : BaseController
    {
        private readonly IHomepageService _homepageService;

        public HomepageController(IHomepageService homepageService)
        {
            _homepageService = homepageService;
        }

        // ==================== GET /api/v1/homepage ====================

        /// <summary>
        /// Get Homepage Blocks
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<GetHomepageResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHomepage(
            [FromQuery] string? city = null,
            [FromQuery] string language = "ar",
            [FromQuery] string? deviceType = null)
        {
            try
            {
                var request = new GetHomepageRequest
                {
                    UserId = UserId
                };

                var response = await _homepageService.GetHomepageAsync(request);

                return Ok(new ResponseModel<GetHomepageResponse>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Homepage loaded successfully.",
                    Data = response
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseModel<object>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Error loading homepage.",
                        ErrorCode = "HOMEPAGE_ERROR"
                    });
            }
        }

        // ==================== GET /api/v1/homepage/block/{blockId} ====================

        /// <summary>
        /// Get Specific Homepage Block
        /// </summary>
        [HttpGet("block/{blockId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<HomepageBlockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlock(
            Guid blockId,
            [FromQuery] string language = "ar")
        {
            if (blockId == Guid.Empty)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid block ID",
                    ErrorCode = "INVALID_BLOCK_ID"
                });
            }

            var block = await _homepageService.GetBlockByIdAsync(blockId);

            if (block == null)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Block not found.",
                    ErrorCode = "BLOCK_NOT_FOUND"
                });
            }

            return Ok(new ResponseModel<HomepageBlockDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Block retrieved successfully.",
                Data = block
            });
        }
    }
}