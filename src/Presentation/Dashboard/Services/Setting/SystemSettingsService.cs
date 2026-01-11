using Common.Enumerations.Settings;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Setting;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Services.Setting
{
	public class SystemSettingsService : ISystemSettingsService
	{
		private readonly IApiService _apiService;
		private readonly ILogger<SystemSettingsService> _logger;

		public SystemSettingsService(
			IApiService apiService,
			ILogger<SystemSettingsService> logger)
		{
			_apiService = apiService;
			_logger = logger;
		}

		#region Get Settings by Type

		public async Task<ResponseModel<decimal>> GetDecimalSettingAsync(SystemSettingKey key)
		{
			try
			{
				return await _apiService.GetAsync<decimal>(
					ApiEndpoints.SystemSettings.GetDecimal((int)key)
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting decimal setting {Key}", key);
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<int>> GetIntSettingAsync(SystemSettingKey key)
		{
			try
			{
				return await _apiService.GetAsync<int>(
					ApiEndpoints.SystemSettings.GetInt((int)key)
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting int setting {Key}", key);
				return new ResponseModel<int>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> GetBoolSettingAsync(SystemSettingKey key)
		{
			try
			{
				return await _apiService.GetAsync<bool>(
					ApiEndpoints.SystemSettings.GetBool((int)key)
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting bool setting {Key}", key);
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<string>> GetStringSettingAsync(SystemSettingKey key)
		{
			try
			{
				return await _apiService.GetAsync<string>(
					ApiEndpoints.SystemSettings.GetString((int)key)
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting string setting {Key}", key);
				return new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<DateTime>> GetDateTimeSettingAsync(SystemSettingKey key)
		{
			try
			{
				return await _apiService.GetAsync<DateTime>(
					ApiEndpoints.SystemSettings.GetDateTime((int)key)
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting datetime setting {Key}", key);
				return new ResponseModel<DateTime>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		#endregion

		#region Update Settings

		public async Task<ResponseModel<bool>> UpdateSettingAsync(UpdateSystemSettingDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			try
			{
				return await _apiService.PutAsync<UpdateSystemSettingDto, bool>(
					ApiEndpoints.SystemSettings.Update,
					dto
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating setting {Key}", dto.key);
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> UpdateSettingsBatchAsync(List<UpdateSystemSettingDto> dtos)
		{
			if (dtos == null || !dtos.Any())
				throw new ArgumentNullException(nameof(dtos));

			try
			{
				return await _apiService.PutAsync<List<UpdateSystemSettingDto>, bool>(
					ApiEndpoints.SystemSettings.UpdateBatch,
					dtos
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating settings batch");
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		#endregion

		#region Specific Business Settings

		public async Task<ResponseModel<decimal>> GetTaxRateAsync()
		{
			try
			{
				return await _apiService.GetAsync<decimal>(
					ApiEndpoints.SystemSettings.TaxRate
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting tax rate");
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<decimal>> GetFreeShippingThresholdAsync()
		{
			try
			{
				return await _apiService.GetAsync<decimal>(
					ApiEndpoints.SystemSettings.FreeShippingThreshold
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting free shipping threshold");
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> IsCashOnDeliveryEnabledAsync()
		{
			try
			{
				return await _apiService.GetAsync<bool>(
					ApiEndpoints.SystemSettings.CashOnDeliveryEnabled
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking cash on delivery status");
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> IsMaintenanceModeAsync()
		{
			try
			{
				return await _apiService.GetAsync<bool>(
					ApiEndpoints.SystemSettings.MaintenanceMode
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking maintenance mode");
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<decimal>> GetMinimumOrderAmountAsync()
		{
			try
			{
				return await _apiService.GetAsync<decimal>(
					ApiEndpoints.SystemSettings.MinimumOrderAmount
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting minimum order amount");
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		#endregion

		#region Get All Settings

		public async Task<ResponseModel<SystemSettingsViewModelDto>> GetAllSettingsAsync()
		{
			try
			{
				var viewModel = new SystemSettingsViewModelDto();

				// Tax Settings
				var taxRate = await GetDecimalSettingAsync(SystemSettingKey.OrderTaxPercentage);
				if (taxRate.Success) viewModel.OrderTaxPercentage = taxRate.Data;

				var taxIncluded = await GetBoolSettingAsync(SystemSettingKey.TaxIncludedInPrice);
				if (taxIncluded.Success) viewModel.TaxIncludedInPrice = taxIncluded.Data;

				var refundDays = await GetIntSettingAsync(SystemSettingKey.RefundAllowedDays);
				if (refundDays.Success) viewModel.RefundAllowedDays = refundDays.Data;


				// Shipping Settings
				//var shippingAmount = await GetDecimalSettingAsync(SystemSettingKey.ShippingAmount);
				//if (shippingAmount.Success) viewModel.ShippingAmount = shippingAmount.Data;

				//var freeShipping = await GetDecimalSettingAsync(SystemSettingKey.FreeShippingThreshold);
				//if (freeShipping.Success) viewModel.FreeShippingThreshold = freeShipping.Data;

				//var shippingPerKg = await GetDecimalSettingAsync(SystemSettingKey.ShippingPerKg);
				//if (shippingPerKg.Success) viewModel.ShippingPerKg = shippingPerKg.Data;

				//var deliveryDays = await GetIntSettingAsync(SystemSettingKey.EstimatedDeliveryDays);
				//if (deliveryDays.Success) viewModel.EstimatedDeliveryDays = deliveryDays.Data;

				//// Order Settings
				//var extraCost = await GetDecimalSettingAsync(SystemSettingKey.OrderExtraCost);
				//if (extraCost.Success) viewModel.OrderExtraCost = extraCost.Data;

				//var minOrder = await GetDecimalSettingAsync(SystemSettingKey.MinimumOrderAmount);
				//if (minOrder.Success) viewModel.MinimumOrderAmount = minOrder.Data;

				//var maxOrder = await GetDecimalSettingAsync(SystemSettingKey.MaximumOrderAmount);
				//if (maxOrder.Success) viewModel.MaximumOrderAmount = maxOrder.Data;

				//var cancelPeriod = await GetIntSettingAsync(SystemSettingKey.OrderCancellationPeriodHours);
				//if (cancelPeriod.Success) viewModel.OrderCancellationPeriodHours = cancelPeriod.Data;

				//// Payment Settings
				//var paymentEnabled = await GetBoolSettingAsync(SystemSettingKey.PaymentGatewayEnabled);
				//if (paymentEnabled.Success) viewModel.PaymentGatewayEnabled = paymentEnabled.Data;

				//var codEnabled = await GetBoolSettingAsync(SystemSettingKey.CashOnDeliveryEnabled);
				//if (codEnabled.Success) viewModel.CashOnDeliveryEnabled = codEnabled.Data;

				//// Business Settings
				//var maintenance = await GetBoolSettingAsync(SystemSettingKey.MaintenanceMode);
				//if (maintenance.Success) viewModel.MaintenanceMode = maintenance.Data;

				//var guestCheckout = await GetBoolSettingAsync(SystemSettingKey.AllowGuestCheckout);
				//if (guestCheckout.Success) viewModel.AllowGuestCheckout = guestCheckout.Data;

				//// Notification Settings
				//var emailNotif = await GetBoolSettingAsync(SystemSettingKey.EmailNotificationsEnabled);
				//if (emailNotif.Success) viewModel.EmailNotificationsEnabled = emailNotif.Data;

				//var smsNotif = await GetBoolSettingAsync(SystemSettingKey.SmsNotificationsEnabled);
				//if (smsNotif.Success) viewModel.SmsNotificationsEnabled = smsNotif.Data;

				//// Security Settings
				//var maxLogin = await GetIntSettingAsync(SystemSettingKey.MaxLoginAttempts);
				//if (maxLogin.Success) viewModel.MaxLoginAttempts = maxLogin.Data;

				//var sessionTimeout = await GetIntSettingAsync(SystemSettingKey.SessionTimeoutMinutes);
				//if (sessionTimeout.Success) viewModel.SessionTimeoutMinutes = sessionTimeout.Data;

				//var passwordLength = await GetIntSettingAsync(SystemSettingKey.PasswordMinLength);
				//if (passwordLength.Success) viewModel.PasswordMinLength = passwordLength.Data;

				return new ResponseModel<SystemSettingsViewModelDto>
				{
					Success = true,
					Data = viewModel,
					Message = "Settings loaded successfully"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading all settings");
				return new ResponseModel<SystemSettingsViewModelDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		#endregion
	}
}


//using Common.Enumerations.Settings;
//using Dashboard.Contracts.Setting;
//using Shared.DTOs.Setting;
//using Shared.GeneralModels;
//using System.Net.Http.Json;

//namespace Dashboard.Services.Setting
//{
//	public class SystemSettingsService : ISystemSettingsService
//	{
//		private readonly HttpClient _httpClient;
//		private readonly ILogger<SystemSettingsService> _logger;
//		private const string BaseEndpoint = "api/v1/SystemSettings";

//		public SystemSettingsService(
//			HttpClient httpClient,
//			ILogger<SystemSettingsService> logger)
//		{
//			_httpClient = httpClient;
//			_logger = logger;
//		}

//		#region Get Settings by Type

//		public async Task<ResponseModel<decimal>> GetDecimalSettingAsync(SystemSettingKey key)
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<decimal>>(
//					$"{BaseEndpoint}/decimal/{(int)key}");
//				return response ?? new ResponseModel<decimal> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting decimal setting {Key}", key);
//				return new ResponseModel<decimal>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<int>> GetIntSettingAsync(SystemSettingKey key)
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<int>>(
//					$"{BaseEndpoint}/int/{(int)key}");
//				return response ?? new ResponseModel<int> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting int setting {Key}", key);
//				return new ResponseModel<int>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<bool>> GetBoolSettingAsync(SystemSettingKey key)
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<bool>>(
//					$"{BaseEndpoint}/bool/{(int)key}");
//				return response ?? new ResponseModel<bool> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting bool setting {Key}", key);
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<string>> GetStringSettingAsync(SystemSettingKey key)
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<string>>(
//					$"{BaseEndpoint}/string/{(int)key}");
//				return response ?? new ResponseModel<string> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting string setting {Key}", key);
//				return new ResponseModel<string>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<DateTime>> GetDateTimeSettingAsync(SystemSettingKey key)
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<DateTime>>(
//					$"{BaseEndpoint}/datetime/{(int)key}");
//				return response ?? new ResponseModel<DateTime> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting datetime setting {Key}", key);
//				return new ResponseModel<DateTime>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		#endregion

//		#region Update Settings

//		public async Task<ResponseModel<bool>> UpdateSettingAsync(UpdateSystemSettingDto dto)
//		{
//			try
//			{
//				var response = await _httpClient.PutAsJsonAsync($"{BaseEndpoint}/update", dto);

//				if (response.IsSuccessStatusCode)
//				{
//					var result = await response.Content.ReadFromJsonAsync<ResponseModel<bool>>();
//					return result ?? new ResponseModel<bool> { Success = false };
//				}

//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = $"Request failed with status code: {response.StatusCode}"
//				};
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error updating setting {Key}", dto.Key);
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<bool>> UpdateSettingsBatchAsync(List<UpdateSystemSettingDto> dtos)
//		{
//			try
//			{
//				var response = await _httpClient.PutAsJsonAsync($"{BaseEndpoint}/update-batch", dtos);

//				if (response.IsSuccessStatusCode)
//				{
//					var result = await response.Content.ReadFromJsonAsync<ResponseModel<bool>>();
//					return result ?? new ResponseModel<bool> { Success = false };
//				}

//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = $"Request failed with status code: {response.StatusCode}"
//				};
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error updating settings batch");
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		#endregion

//		#region Specific Business Settings

//		public async Task<ResponseModel<decimal>> GetTaxRateAsync()
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<decimal>>(
//					$"{BaseEndpoint}/tax-rate");
//				return response ?? new ResponseModel<decimal> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting tax rate");
//				return new ResponseModel<decimal>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<decimal>> GetFreeShippingThresholdAsync()
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<decimal>>(
//					$"{BaseEndpoint}/free-shipping-threshold");
//				return response ?? new ResponseModel<decimal> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting free shipping threshold");
//				return new ResponseModel<decimal>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<bool>> IsCashOnDeliveryEnabledAsync()
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<bool>>(
//					$"{BaseEndpoint}/cash-on-delivery-enabled");
//				return response ?? new ResponseModel<bool> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error checking cash on delivery status");
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<bool>> IsMaintenanceModeAsync()
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<bool>>(
//					$"{BaseEndpoint}/maintenance-mode");
//				return response ?? new ResponseModel<bool> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error checking maintenance mode");
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		public async Task<ResponseModel<decimal>> GetMinimumOrderAmountAsync()
//		{
//			try
//			{
//				var response = await _httpClient.GetFromJsonAsync<ResponseModel<decimal>>(
//					$"{BaseEndpoint}/minimum-order-amount");
//				return response ?? new ResponseModel<decimal> { Success = false };
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error getting minimum order amount");
//				return new ResponseModel<decimal>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		#endregion

//		#region Get All Settings

//		public async Task<ResponseModel<SystemSettingsViewModelDto>> GetAllSettingsAsync()
//		{
//			try
//			{
//				var viewModel = new SystemSettingsViewModelDto();

//				// Tax Settings
//				var taxRate = await GetDecimalSettingAsync(SystemSettingKey.OrderTaxPercentage);
//				if (taxRate.Success) viewModel.OrderTaxPercentage = taxRate.Data;

//				var taxIncluded = await GetBoolSettingAsync(SystemSettingKey.TaxIncludedInPrice);
//				if (taxIncluded.Success) viewModel.TaxIncludedInPrice = taxIncluded.Data;

//				// Shipping Settings
//				var shippingAmount = await GetDecimalSettingAsync(SystemSettingKey.ShippingAmount);
//				if (shippingAmount.Success) viewModel.ShippingAmount = shippingAmount.Data;

//				var freeShipping = await GetDecimalSettingAsync(SystemSettingKey.FreeShippingThreshold);
//				if (freeShipping.Success) viewModel.FreeShippingThreshold = freeShipping.Data;

//				var shippingPerKg = await GetDecimalSettingAsync(SystemSettingKey.ShippingPerKg);
//				if (shippingPerKg.Success) viewModel.ShippingPerKg = shippingPerKg.Data;

//				var deliveryDays = await GetIntSettingAsync(SystemSettingKey.EstimatedDeliveryDays);
//				if (deliveryDays.Success) viewModel.EstimatedDeliveryDays = deliveryDays.Data;

//				// Order Settings
//				var extraCost = await GetDecimalSettingAsync(SystemSettingKey.OrderExtraCost);
//				if (extraCost.Success) viewModel.OrderExtraCost = extraCost.Data;

//				var minOrder = await GetDecimalSettingAsync(SystemSettingKey.MinimumOrderAmount);
//				if (minOrder.Success) viewModel.MinimumOrderAmount = minOrder.Data;

//				var maxOrder = await GetDecimalSettingAsync(SystemSettingKey.MaximumOrderAmount);
//				if (maxOrder.Success) viewModel.MaximumOrderAmount = maxOrder.Data;

//				var cancelPeriod = await GetIntSettingAsync(SystemSettingKey.OrderCancellationPeriodHours);
//				if (cancelPeriod.Success) viewModel.OrderCancellationPeriodHours = cancelPeriod.Data;

//				// Payment Settings
//				var paymentEnabled = await GetBoolSettingAsync(SystemSettingKey.PaymentGatewayEnabled);
//				if (paymentEnabled.Success) viewModel.PaymentGatewayEnabled = paymentEnabled.Data;

//				var codEnabled = await GetBoolSettingAsync(SystemSettingKey.CashOnDeliveryEnabled);
//				if (codEnabled.Success) viewModel.CashOnDeliveryEnabled = codEnabled.Data;

//				// Business Settings
//				var maintenance = await GetBoolSettingAsync(SystemSettingKey.MaintenanceMode);
//				if (maintenance.Success) viewModel.MaintenanceMode = maintenance.Data;

//				var guestCheckout = await GetBoolSettingAsync(SystemSettingKey.AllowGuestCheckout);
//				if (guestCheckout.Success) viewModel.AllowGuestCheckout = guestCheckout.Data;

//				// Notification Settings
//				var emailNotif = await GetBoolSettingAsync(SystemSettingKey.EmailNotificationsEnabled);
//				if (emailNotif.Success) viewModel.EmailNotificationsEnabled = emailNotif.Data;

//				var smsNotif = await GetBoolSettingAsync(SystemSettingKey.SmsNotificationsEnabled);
//				if (smsNotif.Success) viewModel.SmsNotificationsEnabled = smsNotif.Data;

//				// Security Settings
//				var maxLogin = await GetIntSettingAsync(SystemSettingKey.MaxLoginAttempts);
//				if (maxLogin.Success) viewModel.MaxLoginAttempts = maxLogin.Data;

//				var sessionTimeout = await GetIntSettingAsync(SystemSettingKey.SessionTimeoutMinutes);
//				if (sessionTimeout.Success) viewModel.SessionTimeoutMinutes = sessionTimeout.Data;

//				var passwordLength = await GetIntSettingAsync(SystemSettingKey.PasswordMinLength);
//				if (passwordLength.Success) viewModel.PasswordMinLength = passwordLength.Data;

//				return new ResponseModel<SystemSettingsViewModelDto>
//				{
//					Success = true,
//					Data = viewModel,
//					Message = "Settings loaded successfully"
//				};
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error loading all settings");
//				return new ResponseModel<SystemSettingsViewModelDto>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		#endregion
//	}
//}