using Dashboard.Contracts.General;

namespace Dashboard.Services.General
{
    /// <summary>
    /// Dashboard implementation of DateTime service
    /// Converts UTC times from API to local timezone (Egypt Standard Time by default)
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        /// <summary>
        /// Default timezone for the application (Egypt Standard Time)
        /// </summary>
        private const string DefaultTimeZoneId = "Egypt Standard Time";

        /// <summary>
        /// Convert UTC DateTime to local time
        /// Returns formatted date string (yyyy-MM-dd) in local timezone
        /// </summary>
        public string FormatToEgyptianDate(DateTime dateTime)
        {
            try
            {
                var egyptianTime = ConvertToEgyptianTime(dateTime);
                return egyptianTime.ToString("yyyy-MM-dd");
            }
            catch
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Convert UTC DateTime to local time (nullable)
        /// Returns formatted date string or "-" if null
        /// </summary>
        public string FormatToEgyptianDate(DateTime? dateTime)
        {
            if (dateTime == null)
                return "-";

            return FormatToEgyptianDate(dateTime.Value);
        }

        /// <summary>
        /// Convert UTC DateTime to local time and format with time
        /// Returns formatted datetime string (yyyy-MM-dd HH:mm:ss) in local timezone
        /// </summary>
        public string FormatToEgyptianDateTime(DateTime dateTime)
        {
            try
            {
                var egyptianTime = ConvertToEgyptianTime(dateTime);
                return egyptianTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// Convert UTC DateTime to local time and format with time (nullable)
        /// Returns formatted datetime string or "-" if null
        /// </summary>
        public string FormatToEgyptianDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
                return "-";

            return FormatToEgyptianDateTime(dateTime.Value);
        }

        /// <summary>
        /// Internal helper to convert UTC time to Egyptian time
        /// </summary>
        private DateTime ConvertToEgyptianTime(DateTime utcDateTime)
        {
            try
            {
                // Ensure the input is UTC
                if (utcDateTime.Kind != DateTimeKind.Utc)
                {
                    utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                }

                // Get Egyptian timezone
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);

                // Convert to Egyptian time
                return TimeZoneInfo.ConvertTime(utcDateTime, egyptTimeZone);
            }
            catch
            {
                // Fallback: return UTC+2 (Egypt Standard Time offset)
                return utcDateTime.AddHours(2);
            }
        }
    }
}
