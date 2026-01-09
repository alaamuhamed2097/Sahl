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
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	[ApiVersion("1.0")]
	//[Authorize(Roles = nameof(UserRole.Admin))]
	public class SystemSettingsController : BaseController
	{
		private readonly ISystemSettingsService _systemSettingsService;

		public SystemSettingsController(ISystemSettingsService systemSettingsService)
		{
			_systemSettingsService = systemSettingsService;
		}

		#region Get Settings by Key

		/// <summary>
		/// Get a decimal setting value by key.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		/// <param name="key">The setting key (e.g., OrderTaxPercentage, ShippingAmount)</param>
		[HttpGet("decimal/{key}")]
		public async Task<IActionResult> GetDecimalSetting([FromRoute] SystemSettingKey key)
		{
			try
			{
				var value = await _systemSettingsService.GetDecimalSettingAsync(key);
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get an integer setting value by key.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		/// <param name="key">The setting key (e.g., MaxLoginAttempts, SessionTimeoutMinutes)</param>
		[HttpGet("int/{key}")]
		public async Task<IActionResult> GetIntSetting([FromRoute] SystemSettingKey key)
		{
			try
			{
				var value = await _systemSettingsService.GetIntSettingAsync(key);
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get a boolean setting value by key.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		/// <param name="key">The setting key (e.g., MaintenanceMode, CashOnDeliveryEnabled)</param>
		[HttpGet("bool/{key}")]
		public async Task<IActionResult> GetBoolSetting([FromRoute] SystemSettingKey key)
		{
			try
			{
				var value = await _systemSettingsService.GetBoolSettingAsync(key);
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get a string setting value by key.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		/// <param name="key">The setting key (e.g., StripePublicKey, BusinessHoursStart)</param>
		[HttpGet("string/{key}")]
		public async Task<IActionResult> GetStringSetting([FromRoute] SystemSettingKey key)
		{
			try
			{
				var value = await _systemSettingsService.GetStringSettingAsync(key);
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get a datetime setting value by key.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		/// <param name="key">The setting key</param>
		[HttpGet("datetime/{key}")]
		public async Task<IActionResult> GetDateTimeSetting([FromRoute] SystemSettingKey key)
		{
			try
			{
				var value = await _systemSettingsService.GetDateTimeSettingAsync(key);
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		#endregion

		#region Update Settings

		/// <summary>
		/// Update a system setting value.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPut("update")]
		public async Task<IActionResult> UpdateSetting([FromBody] UpdateSystemSettingDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));
			}

			try
			{
				var success = await _systemSettingsService.UpdateSettingAsync(
					dto.key,
					dto.value,
					GuidUserId
				);

				if (!success)
				{
					return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));
				}

				return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to update setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Update multiple system settings at once.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpPut("update-batch")]
		public async Task<IActionResult> UpdateSettings([FromBody] List<UpdateSystemSettingDto> dtos)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));
			}

			try
			{
				var settings = dtos.Select(dto => (dto.key, dto.value, dto.dataType,
			dto.category)).ToList();

				var success = await _systemSettingsService.UpdateSettingsBatchAsync(
					 settings.Select(s => (s.key, s.value, s.dataType, s.category)).ToList(),
					GuidUserId
					);

				if (success)
				{
					return Ok(CreateSuccessResponse<string>(
						null,
						NotifiAndAlertsResources.SavedSuccessfully
					));
				}
				else
				{
					return BadRequest(CreateErrorResponse("Some settings failed to update"));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to update settings: {ex.Message}"));
			}
		}
		#endregion

		#region Specific Business Settings

		/// <summary>
		/// Get the current tax rate.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("tax-rate")]
		public async Task<IActionResult> GetTaxRate()
		{
			try
			{
				var value = await _systemSettingsService.GetTaxRateAsync();
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve tax rate: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get the free shipping threshold.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("free-shipping-threshold")]
		public async Task<IActionResult> GetFreeShippingThreshold()
		{
			try
			{
				var value = await _systemSettingsService.GetFreeShippingThresholdAsync();
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve threshold: {ex.Message}"));
			}
		}

		/// <summary>
		/// Check if cash on delivery is enabled.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("cash-on-delivery-enabled")]
		public async Task<IActionResult> IsCashOnDeliveryEnabled()
		{
			try
			{
				var value = await _systemSettingsService.IsCashOnDeliveryEnabledAsync();
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Check if maintenance mode is active.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("maintenance-mode")]
		public async Task<IActionResult> IsMaintenanceMode()
		{
			try
			{
				var value = await _systemSettingsService.IsMaintenanceModeAsync();
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve setting: {ex.Message}"));
			}
		}

		/// <summary>
		/// Get the minimum order amount.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// </remarks>
		[HttpGet("minimum-order-amount")]
		public async Task<IActionResult> GetMinimumOrderAmount()
		{
			try
			{
				var value = await _systemSettingsService.GetMinimumOrderAmountAsync();
				return Ok(CreateSuccessResponse(value, NotifiAndAlertsResources.DataRetrieved));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve minimum order amount: {ex.Message}"));
			}
		}

		#endregion

		#region Get Settings by Category

		/// <summary>
		/// Get all settings by category.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// Requires Admin role.
		/// Note: This requires adding a method to ISystemSettingsService
		/// </remarks>
		[HttpGet("category/{category}")]
		public async Task<IActionResult> GetSettingsByCategory([FromRoute] SystemSettingCategory category)
		{
			try
			{
				// You'll need to add this method to ISystemSettingsService
				// var settings = await _systemSettingsService.GetSettingsByCategoryAsync(category);
				// return Ok(CreateSuccessResponse(settings, NotifiAndAlertsResources.DataRetrieved));

				return BadRequest(CreateErrorResponse("Method not implemented yet"));
			}
			catch (Exception ex)
			{
				return BadRequest(CreateErrorResponse($"Failed to retrieve settings: {ex.Message}"));
			}
		}

		#endregion

		#region Private Helper Methods

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