namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Paginated response for block categories
    /// </summary>
    public class BlockCategoriesPagedResponse
    {
        public Guid BlockId { get; set; }
        public string BlockTitleAr { get; set; } = string.Empty;
        public string BlockTitleEn { get; set; } = string.Empty;
        public string BlockType { get; set; } = string.Empty;
        public List<CategoryCardDto> Categories { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}