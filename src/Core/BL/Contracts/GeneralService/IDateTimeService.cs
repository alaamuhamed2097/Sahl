namespace BL.Contracts.GeneralService;

/// <summary>
/// Service for handling date/time conversions and formatting
/// خدمة للتعامل مع تحويلات التاريخ والوقت والتنسيق
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Convert UTC DateTime to local timezone configured in system settings
    /// تحويل وقت UTC إلى المنطقة الزمنية المحلية المحددة في إعدادات النظام
    /// </summary>
    DateTime ConvertToLocalTime(DateTime utcDateTime);

    /// <summary>
    /// Convert UTC DateTime to local timezone (nullable)
    /// تحويل وقت UTC إلى المنطقة الزمنية المحلية (قابل للقيمة الفارغة)
    /// </summary>
    DateTime? ConvertToLocalTime(DateTime? utcDateTime);

    /// <summary>
    /// Format DateTime to date format (yyyy-MM-dd)
    /// تنسيق التاريخ والوقت إلى صيغة التاريخ
    /// </summary>
    string FormatToDate(DateTime dateTime);

    /// <summary>
    /// Format DateTime to date format (nullable)
    /// تنسيق التاريخ والوقت إلى صيغة التاريخ (قابل للقيمة الفارغة)
    /// </summary>
    string FormatToDate(DateTime? dateTime);

    /// <summary>
    /// Format DateTime to date and time format (yyyy-MM-dd HH:mm:ss)
    /// تنسيق التاريخ والوقت إلى صيغة التاريخ والوقت
    /// </summary>
    string FormatToDateTime(DateTime dateTime);

    /// <summary>
    /// Format DateTime to date and time format (nullable)
    /// تنسيق التاريخ والوقت إلى صيغة التاريخ والوقت (قابل للقيمة الفارغة)
    /// </summary>
    string FormatToDateTime(DateTime? dateTime);

    /// <summary>
    /// Get the current timezone ID from system settings
    /// الحصول على معرف المنطقة الزمنية الحالية من إعدادات النظام
    /// </summary>
    Task<string> GetTimeZoneIdAsync();

    /// <summary>
    /// Convert local time to UTC
    /// تحويل الوقت المحلي إلى UTC
    /// </summary>
    Task<DateTime> ConvertToUtcAsync(DateTime localDateTime);

    /// <summary>
    /// Convert local time to UTC (nullable)
    /// تحويل الوقت المحلي إلى UTC (قابل للقيمة الفارغة)
    /// </summary>
    Task<DateTime?> ConvertToUtcAsync(DateTime? localDateTime);
}
