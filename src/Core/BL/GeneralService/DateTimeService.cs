using BL.Contracts.GeneralService;
using BL.Contracts.Service.Setting;
using Common.Enumerations.Settings;
using Serilog;

namespace BL.GeneralService;

/// <summary>
/// Service for handling date/time conversions and formatting with configurable timezone
/// </summary>
public class DateTimeService : IDateTimeService
{
    private readonly ISystemSettingsService _systemSettingsService;
    private readonly ILogger _logger;
    private string? _cachedTimeZoneId;

    /// <summary>
    /// Default timezone (Egypt Standard Time)
    /// </summary>
    private const string DefaultTimeZoneId = "Egypt Standard Time";

    public DateTimeService(
        ISystemSettingsService systemSettingsService,
        ILogger logger)
    {
        _systemSettingsService = systemSettingsService ?? throw new ArgumentNullException(nameof(systemSettingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Convert UTC DateTime to local timezone configured in system settings
    /// </summary>
    public DateTime ConvertToLocalTime(DateTime utcDateTime)
    {
        try
        {
            // Ensure the input is UTC
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }

            // Get timezone ID (use cached value if available)
            var timeZoneId = _cachedTimeZoneId ?? DefaultTimeZoneId;

            // Get timezone info
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Convert to local time
            return TimeZoneInfo.ConvertTime(utcDateTime, timeZone);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error converting to local time. Using default offset (UTC+2)");
            // Fallback: return UTC+2 (Egypt Standard Time offset)
            return utcDateTime.AddHours(2);
        }
    }

    /// <summary>
    /// Convert UTC DateTime to local timezone (nullable)
    /// </summary>
    public DateTime? ConvertToLocalTime(DateTime? utcDateTime)
    {
        if (utcDateTime == null)
            return null;

        return ConvertToLocalTime(utcDateTime.Value);
    }

    /// <summary>
    /// Format DateTime to date format (yyyy-MM-dd)
    /// </summary>
    public string FormatToDate(DateTime dateTime)
    {
        var localTime = ConvertToLocalTime(dateTime);
        return localTime.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Format DateTime to date format (nullable)
    /// </summary>
    public string FormatToDate(DateTime? dateTime)
    {
        if (dateTime == null)
            return "-";

        return FormatToDate(dateTime.Value);
    }

    /// <summary>
    /// Format DateTime to date and time format (yyyy-MM-dd HH:mm:ss)
    /// </summary>
    public string FormatToDateTime(DateTime dateTime)
    {
        var localTime = ConvertToLocalTime(dateTime);
        return localTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Format DateTime to date and time format (nullable)
    /// </summary>
    public string FormatToDateTime(DateTime? dateTime)
    {
        if (dateTime == null)
            return "-";

        return FormatToDateTime(dateTime.Value);
    }

    /// <summary>
    /// Get the current timezone ID from system settings
    /// </summary>
    public async Task<string> GetTimeZoneIdAsync()
    {
        try
        {
            // Try to get from cache first
            if (!string.IsNullOrEmpty(_cachedTimeZoneId))
            {
                return _cachedTimeZoneId;
            }

            // Get from system settings
            var timeZoneId = await _systemSettingsService.GetStringSettingAsync(SystemSettingKey.SystemTimeZoneId);

            // Use default if not configured
            _cachedTimeZoneId = string.IsNullOrWhiteSpace(timeZoneId) ? DefaultTimeZoneId : timeZoneId;

            return _cachedTimeZoneId;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving timezone from settings. Using default.");
            _cachedTimeZoneId = DefaultTimeZoneId;
            return _cachedTimeZoneId;
        }
    }

    /// <summary>
    /// Convert local time to UTC
    /// </summary>
    public async Task<DateTime> ConvertToUtcAsync(DateTime localDateTime)
    {
        try
        {
            var timeZoneId = await GetTimeZoneIdAsync();
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Ensure the input is local time
            if (localDateTime.Kind == DateTimeKind.Utc)
            {
                return localDateTime;
            }

            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);

            // Convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error converting to UTC. Using offset calculation (UTC+2)");
            // Fallback: subtract 2 hours (Egypt Standard Time offset)
            return localDateTime.Subtract(TimeSpan.FromHours(2));
        }
    }

    /// <summary>
    /// Convert local time to UTC (nullable)
    /// </summary>
    public async Task<DateTime?> ConvertToUtcAsync(DateTime? localDateTime)
    {
        if (localDateTime == null)
            return null;

        return await ConvertToUtcAsync(localDateTime.Value);
    }
}
