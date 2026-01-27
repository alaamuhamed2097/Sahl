using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;
using System.Security.Claims;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Homepage Controller - Public facing API
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Homepage")]
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
        public async Task<IActionResult> GetHomepage()
        {
            var response = await _homepageService.GetHomepageAsync(UserId);

            return Ok(new ResponseModel<GetHomepageResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Homepage loaded successfully.",
                Data = response
            });
        }

        /// <summary>
        /// Get block items with pagination (for items blocks only)
        /// </summary>
        [HttpGet("blocks/{blockId}/items")]
        public async Task<IActionResult> GetBlockItems(
            Guid blockId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _homepageService.GetBlockItemsAsync(blockId, userId, pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Get block categories with pagination (for category blocks only)
        /// </summary>
        [HttpGet("blocks/{blockId}/categories")]
        public async Task<IActionResult> GetBlockCategories(
            Guid blockId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _homepageService.GetBlockCategoriesAsync(blockId, pageNumber, pageSize);
            return Ok(result);
        }
    }
}