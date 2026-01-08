using BL.Contracts.Service.Setting;
using Common.Enumerations.Settings;
using DAL.Contracts.Repositories.Configuration;
using Serilog;

namespace BL.Services.Setting;

/// <summary>
/// Service for managing system settings with caching support
/// </summary>
public class SystemSettingsService : ISystemSettingsService
{
    private readonly ISystemSettingsRepository _repository;
    private readonly ILogger _logger;

    public SystemSettingsService(
        ISystemSettingsRepository repository,
        ILogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<decimal> GetDecimalSettingAsync(SystemSettingKey key)
    {
        var value = await _repository.GetDecimalSettingAsync(key);

        return value;
    }

    public async Task<int> GetIntSettingAsync(SystemSettingKey key)
    {
        var value = await _repository.GetIntSettingAsync(key);

        return value;
    }

    public async Task<bool> GetBoolSettingAsync(SystemSettingKey key)
    {
        var value = await _repository.GetBoolSettingAsync(key);

        return value;
    }

    public async Task<string?> GetStringSettingAsync(SystemSettingKey key)
    {
        var value = await _repository.GetSettingValueAsync(key);

        return value;
    }

    public async Task<DateTime> GetDateTimeSettingAsync(SystemSettingKey key)
    {
        var value = await _repository.GetDateTimeSettingAsync(key);

        return value;
    }

    public async Task<bool> UpdateSettingAsync(SystemSettingKey key, string value, Guid updatedBy)
    {
        var result = await _repository.UpdateSettingAsync(key, value, updatedBy);

        if (result)
        {
            _logger.Information("Setting {SettingKey} updated successfully", key.GetDescription());
        }

        return result;
    }

    public async Task<decimal> GetTaxRateAsync()
    {
        return await GetDecimalSettingAsync(SystemSettingKey.OrderTaxPercentage);
    }

    public async Task<decimal> GetFreeShippingThresholdAsync()
    {
        return await GetDecimalSettingAsync(SystemSettingKey.FreeShippingThreshold);
    }

    public async Task<bool> IsCashOnDeliveryEnabledAsync()
    {
        return await GetBoolSettingAsync(SystemSettingKey.CashOnDeliveryEnabled);
    }

    public async Task<bool> IsMaintenanceModeAsync()
    {
        return await GetBoolSettingAsync(SystemSettingKey.MaintenanceMode);
    }

    public async Task<decimal> GetMinimumOrderAmountAsync()
    {
        return await GetDecimalSettingAsync(SystemSettingKey.MinimumOrderAmount);
    }
}