using Common.Enumerations.Settings;
using Dashboard.Models.pagintion;
using Shared.DTOs.Order.Payment.Refund;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Setting
{
	public interface ISystemSettingsService
	{
		// Get Settings by Type
		Task<ResponseModel<decimal>> GetDecimalSettingAsync(SystemSettingKey key);
		Task<ResponseModel<int>> GetIntSettingAsync(SystemSettingKey key);
		Task<ResponseModel<bool>> GetBoolSettingAsync(SystemSettingKey key);
		Task<ResponseModel<string>> GetStringSettingAsync(SystemSettingKey key);
		Task<ResponseModel<DateTime>> GetDateTimeSettingAsync(SystemSettingKey key);

		// Update Settings
		Task<ResponseModel<bool>> UpdateSettingAsync(UpdateSystemSettingDto dto);
		Task<ResponseModel<bool>> UpdateSettingsBatchAsync(List<UpdateSystemSettingDto> dtos);

		// Specific Business Settings
		Task<ResponseModel<decimal>> GetTaxRateAsync();
		Task<ResponseModel<decimal>> GetFreeShippingThresholdAsync();
		Task<ResponseModel<bool>> IsCashOnDeliveryEnabledAsync();
		Task<ResponseModel<bool>> IsMaintenanceModeAsync();
		Task<ResponseModel<decimal>> GetMinimumOrderAmountAsync();

		// Get All Settings for UI
		Task<ResponseModel<SystemSettingsViewModelDto>> GetAllSettingsAsync();

		Task<ResponseModel<PaginatedDataModel<RefundRequestDto>>> GetAllRefundsAsync();
		Task<ResponseModel<RefundRequestDto>> GetRefundByIdAsync(Guid refundId);
		Task<ResponseModel<RefundRequestDto>> GetRefundByOrderDetailIdAsync(Guid orderDetailId);
		Task<ResponseModel<bool>> UpdateRefundStatusAsync(UpdateRefundStatusDto dto);
	}
}