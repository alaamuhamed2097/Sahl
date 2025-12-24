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
    }
}