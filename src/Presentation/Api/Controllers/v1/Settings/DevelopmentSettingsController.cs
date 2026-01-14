using Api.Controllers.v1.Base;
using Api.Extensions;
using Asp.Versioning;
using BL.Contracts.Service.Setting;
using Common.Enumerations.Settings;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Settings
{
	[Route("api/v{version:apiVersion}/development-settings")]
	[ApiController]
	[ApiVersion("1.0")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class DevelopmentSettingsController : BaseController
	{
		private readonly IDevelopmentSettingsService _developmentSettingsService;

		public DevelopmentSettingsController(IDevelopmentSettingsService developmentSettingsService)
		{
			_developmentSettingsService = developmentSettingsService;
		}

        /// <summary>
        /// Check if multiVendor mode is active.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("multi-vendor-enabled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsMultiVendorModeEnabled()
		{
			var value = await _developmentSettingsService.IsMultiVendorModeEnabledAsync();
			return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
		}

		/// <summary>
		/// Get development settings.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
		{
			var settings = await _developmentSettingsService.GetAsync();
			return Ok(CreateSuccessResponse(settings, NotifiAndAlertsResources.DataRetrieved));
        }
    }
}