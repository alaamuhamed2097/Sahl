using Common.Enumerations.Settings;
using Domains.Entities.Setting;

namespace DAL.Contracts.Repositories.Configuration;

/// <summary>
/// Repository interface for system settings operations with enum support
/// </summary>
public interface ISystemSettingsRepository : ITableRepository<TbSystemSettings>
{
    /// <summary>
    /// Get setting value as string
    /// </summary>
    Task<string?> GetSettingValueAsync(SystemSettingKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get setting value as decimal (for tax rates, fees, amounts)
    /// </summary>
    Task<decimal> GetDecimalSettingAsync(SystemSettingKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get setting value as integer (for hours, counts, limits)
    /// </summary>
    Task<int> GetIntSettingAsync(SystemSettingKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get setting value as boolean (for enabled/disabled flags)
    /// </summary>
    Task<bool> GetBoolSettingAsync(SystemSettingKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get setting value as datetime (for time-based settings)
    /// </summary>
    Task<DateTime> GetDateTimeSettingAsync(SystemSettingKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update setting value
    /// </summary>
    Task<bool> UpdateSettingAsync(SystemSettingKey key, string value, Guid updatedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all settings by category
    /// </summary>
    Task<List<TbSystemSettings>> GetSettingsByCategoryAsync(SystemSettingCategory category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get full setting entity by key
    /// </summary>
    Task<TbSystemSettings?> GetSettingAsync(SystemSettingKey key, CancellationToken cancellationToken = default);
}