namespace Shared.GeneralModels
{
    /// <summary>
    /// Generic paginated search result model
    /// </summary>
    public class PaginatedSearchResult<T>
    {
        /// <summary>
        /// List of items
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Total number of records (without pagination)
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
