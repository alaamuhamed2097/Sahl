using BL.Contracts.GeneralService;
using Common.Enumerations.Settings;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Configuration;
using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Configuration;

/// <summary>
/// Repository implementation for system settings with enum support
/// </summary>
public class SystemSettingsRepository : TableRepository<TbSystemSettings>, ISystemSettingsRepository
{
    private readonly ApplicationDbContext _context;

    public SystemSettingsRepository(ApplicationDbContext context, ICurrentUserService currentUserService, ILogger logger)
        : base(context, currentUserService, logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<string?> GetSettingValueAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        var setting = await _context.Set<TbSystemSettings>()
            .Where(s => s.SettingKey == key && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return setting?.SettingValue;
    }

    public async Task<decimal> GetDecimalSettingAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        var value = await GetSettingValueAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Setting '{key.GetDescription()}' not found");
        }

        if (!decimal.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Setting '{key.GetDescription()}' value '{value}' is not a valid decimal");
        }

        return result;
    }

    public async Task<int> GetIntSettingAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        var value = await GetSettingValueAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Setting '{key.GetDescription()}' not found");
        }

        if (!int.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Setting '{key.GetDescription()}' value '{value}' is not a valid integer");
        }

        return result;
    }

    public async Task<bool> GetBoolSettingAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        var value = await GetSettingValueAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Setting '{key.GetDescription()}' not found");
        }

        if (!bool.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Setting '{key.GetDescription()}' value '{value}' is not a valid boolean");
        }

        return result;
    }

    public async Task<DateTime> GetDateTimeSettingAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        var value = await GetSettingValueAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Setting '{key.GetDescription()}' not found");
        }

        if (!DateTime.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Setting '{key.GetDescription()}' value '{value}' is not a valid datetime");
        }

        return result;
    }

    public async Task<bool> UpdateSettingAsync(
        SystemSettingKey key,
        string value,
        Guid updatedBy,
        CancellationToken cancellationToken = default)
    {
        var setting = await _context.Set<TbSystemSettings>()
            .Where(s => s.SettingKey == key && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (setting == null)
        {
            throw new InvalidOperationException($"Setting '{key.GetDescription()}' not found");
        }

        setting.SettingValue = value;
        setting.UpdatedBy = updatedBy;
        setting.UpdatedDateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<TbSystemSettings>> GetSettingsByCategoryAsync(
        SystemSettingCategory category,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<TbSystemSettings>()
            .Where(s => s.Category == category && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<TbSystemSettings?> GetSettingAsync(
        SystemSettingKey key,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<TbSystemSettings>()
            .Where(s => s.SettingKey == key && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}