using Api.Controllers.Base;
using Api.Extensions;
using BL.Contracts.Service.Setting;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Api.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService, Serilog.ILogger logger)
            : base(logger)
        {
            _settingService = settingService;
        }

        /// <summary>
        /// Get application settings
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("mainBanner")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainBanner()
        {
            try
            {
                var setting = await _settingService.GetMainBannerPathAsync();
                if (setting == null)
                {
                    return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));
                }

                return Ok(CreateSuccessResponse(setting, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("shippingAmount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShippingAmount()
        {
            try
            {
                var clientIp = HttpContext.GetClientIpAddress();
                var shouldApplyConversion = ShouldApplyCurrencyConversion();
                var setting = await _settingService.GetShippingAmountAsync(clientIp, shouldApplyConversion);

                return Ok(CreateSuccessResponse(setting, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Update application settings
        /// </summary>
        [HttpPost("update")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Update([FromBody] SettingDto dto)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
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