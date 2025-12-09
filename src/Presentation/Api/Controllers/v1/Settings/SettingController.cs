using Api.Controllers.v1.Base;
using Api.Extensions;
using Asp.Versioning;
using BL.Contracts.Service.Setting;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Settings
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        /// <summary>
        /// Get application settings.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var setting = await _settingService.GetSettingsAsync(clientIp, shouldApplyConversion);
            if (setting == null)
            {
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));
            }

            return Ok(CreateSuccessResponse(setting, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Get main banner path.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("mainBanner")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainBanner()
        {
            var setting = await _settingService.GetMainBannerPathAsync();
            if (setting == null)
            {
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));
            }

            return Ok(CreateSuccessResponse(setting, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Get shipping amount.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("shippingAmount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShippingAmount()
        {
            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var setting = await _settingService.GetShippingAmountAsync(clientIp, shouldApplyConversion);

            return Ok(CreateSuccessResponse(setting, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Update application settings.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("update")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Update([FromBody] SettingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));
            }

            var success = await _settingService.UpdateSettingsAsync(dto, GuidUserId);
            if (!success)
            {
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));
            }

            return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        }

        #region Private Helper Methods
        private bool ShouldApplyCurrencyConversion()
        {
            if (RoleName == nameof(UserRole.Admin))
                return false;
            return true;
        }

        private ResponseModel<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ResponseModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        private ResponseModel<string> CreateErrorResponse(string message)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = message
            };
        }
        #endregion
    }
}
