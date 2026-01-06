using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.HomeSlider;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Merchandising
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public class HomePageSliderController : BaseController
    {
        private readonly IHomePageSliderService _homePageSliderService;

        public HomePageSliderController(IHomePageSliderService homePageSliderService, ILogger<HomePageSliderController> logger)
        {
            _homePageSliderService = homePageSliderService;
        }

        /// <summary>
        /// Get slider by ID with full details
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{sliderId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSliderById(Guid sliderId)
        {
            var slider = await _homePageSliderService.FindByIdAsync(sliderId);

            if (slider == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<HomePageSliderDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = slider
            });
        }

        /// <summary>
        /// Get all sliders (Admin only)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("all")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSliders()
        {
            var sliders = await _homePageSliderService.GetAllAsync();

            return Ok(new ResponseModel<IEnumerable<HomePageSliderDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = sliders
            });
        }
    }
}