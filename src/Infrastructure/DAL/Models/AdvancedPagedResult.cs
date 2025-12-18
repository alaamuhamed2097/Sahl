namespace DAL.Models
{
    /// <summary>
    /// Paginated result wrapper
    /// </summary>
    public class AdvancedPagedResult<T>
    {
        /// <summary>
        /// Items in current page
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Whether there's a previous page
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Whether there's a next page
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
