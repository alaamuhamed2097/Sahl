namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Single filter option (category or brand)
    /// </summary>
    public class FilterOptionDto
    {
        /// <summary>
        /// Category/Brand ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Category/Brand name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// Category/Brand name in English
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Number of items matching this filter
        /// </summary>
        public int Count { get; set; }
    }


}
