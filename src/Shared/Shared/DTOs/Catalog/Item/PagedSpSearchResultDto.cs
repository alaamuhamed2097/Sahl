namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Paginated result wrapper for stored procedure search results
    /// </summary>
    public class PagedSpSearchResultDto
    {
        /// <summary>
        /// List of items on current page
        /// </summary>
        public List<SearchItemDto> Items { get; set; } = new List<SearchItemDto>();

        /// <summary>
        /// Total number of items matching search criteria
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page
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