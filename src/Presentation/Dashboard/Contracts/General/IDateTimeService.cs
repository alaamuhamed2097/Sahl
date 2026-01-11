namespace Dashboard.Contracts.General
{
    /// <summary>
    /// Dashboard wrapper for DateTime service operations
    /// This interface provides date/time conversion and formatting functionality
    /// The actual implementation is in the BL layer and accessed through API
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Convert UTC DateTime to local time
        /// Returns formatted date string (yyyy-MM-dd) in local timezone
        /// </summary>
        string FormatToEgyptianDate(DateTime dateTime);

        /// <summary>
        /// Convert UTC DateTime to local time (nullable)
        /// Returns formatted date string or "-" if null
        /// </summary>
        string FormatToEgyptianDate(DateTime? dateTime);

        /// <summary>
        /// Convert UTC DateTime to local time and format with time
        /// Returns formatted datetime string (yyyy-MM-dd HH:mm:ss) in local timezone
        /// </summary>
        string FormatToEgyptianDateTime(DateTime dateTime);

        /// <summary>
        /// Convert UTC DateTime to local time and format with time (nullable)
        /// Returns formatted datetime string or "-" if null
        /// </summary>
        string FormatToEgyptianDateTime(DateTime? dateTime);
    }
}
