using BL.Contracts.Service.Setting;
using Common.Enumerations.Settings;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Configuration;
using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
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

		_logger.Information(
			"Retrieved datetime setting {SettingKey}: {Value}",
			key.GetDescription(),
			value
		);

		return value;
	}
	public async Task<bool> UpdateSettingsBatchAsync(
	List<(SystemSettingKey key, string value, SystemSettingDataType dataType, SystemSettingCategory category)> settings,
	Guid userId)
	{
		try
		{
			foreach (var (key, value, dataType, category) in settings)
			{
				var setting = await _repository.FindAsync(s => s.SettingKey == key && !s.IsDeleted);

				if (setting != null)
				{
					// Update existing setting
					setting.SettingValue = value;
					setting.DataType = dataType;
					setting.Category = category;
					setting.UpdatedBy = userId;
					setting.UpdatedDateUtc = DateTime.UtcNow;
					await _repository.UpdateAsync(setting, userId);

				}
				else
				{
					// Create new setting if not exists
					var newSetting = new TbSystemSettings
					{
						Id = Guid.NewGuid(),
						SettingKey = key,
						SettingValue = value,
						DataType = dataType,
						Category = category,
						IsDeleted = false,
						CreatedBy = userId,
						CreatedDateUtc = DateTime.UtcNow,
						UpdatedBy = userId,
						UpdatedDateUtc = DateTime.UtcNow
					};

					await _repository.CreateAsync(newSetting, userId);
				}
			}

			
			return true;
		}
		catch (Exception ex)
		{
			//_logger.LogError(ex, "Error updating settings batch");
			return false;
		}
	}
	//public async Task<bool> UpdateSettingsBatchAsync(
	//List<(SystemSettingKey Key, string Value)> settings,
	//Guid updatedBy)
	//{
	//	try
	//	{
	//		_logger.Information(
	//			"Batch updating {Count} settings by user {UpdatedBy}",
	//			settings.Count,
	//			updatedBy
	//		);

	//		var tasks = settings.Select(s =>
	//			_repository.UpdateSettingAsync(s.Key, s.Value, updatedBy)
	//		);

	//		var results = await Task.WhenAll(tasks);
	//		var allSucceeded = results.All(r => r);

	//		if (allSucceeded)
	//		{
	//			_logger.Information("All {Count} settings updated successfully", settings.Count);
	//		}
	//		else
	//		{
	//			_logger.Warning("Some settings failed to update in batch operation");
	//		}

	//		return allSucceeded;
	//	}
	//	catch (Exception ex)
	//	{
	//		_logger.Error(ex, "Error during batch settings update");
	//		return false;
	//	}
	//}

	public async Task<bool> UpdateSettingAsync(SystemSettingKey key, string value, Guid updatedBy)
	{
		_logger.Information(
			"Updating setting {SettingKey} to {Value} by user {UpdatedBy}",
			key.GetDescription(),
			value,
			updatedBy
		);

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