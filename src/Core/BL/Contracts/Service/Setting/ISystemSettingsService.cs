using Common.Enumerations.Settings;

namespace BL.Contracts.Service.Setting;

/// <summary>
/// Service interface for system settings management
/// </summary>
public interface ISystemSettingsService
{
    /// <summary>
    /// Get decimal setting value
    /// </summary>
    Task<decimal> GetDecimalSettingAsync(SystemSettingKey key);

    /// <summary>
    /// Get integer setting value
    /// </summary>
    Task<int> GetIntSettingAsync(SystemSettingKey key);

    /// <summary>
    /// Get boolean setting value
    /// </summary>
    Task<bool> GetBoolSettingAsync(SystemSettingKey key);

    /// <summary>
    /// Get string setting value
    /// </summary>
    Task<string?> GetStringSettingAsync(SystemSettingKey key);

    /// <summary>
    /// Get datetime setting value
    /// </summary>
    Task<DateTime> GetDateTimeSettingAsync(SystemSettingKey key);

    /// <summary>
    /// Update setting value
    /// </summary>
    Task<bool> UpdateSettingAsync(SystemSettingKey key, string value, Guid updatedBy);
	//Task<bool> UpdateSettingsBatchAsync(List<(SystemSettingKey Key, string Value)> settings, Guid updatedBy);

	Task<bool> UpdateSettingsBatchAsync(
	   List<(SystemSettingKey key, string value, SystemSettingDataType dataType, SystemSettingCategory category)> settings,
	   Guid userId);
	// Convenience methods for commonly used settings
	Task<decimal> GetTaxRateAsync();
    Task<decimal> GetFreeShippingThresholdAsync();
    Task<bool> IsCashOnDeliveryEnabledAsync();
    Task<bool> IsMaintenanceModeAsync();
    Task<decimal> GetMinimumOrderAmountAsync();
}