namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Single filter option (category, brand, or vendor)
    /// </summary>
    public class FilterOptionDto
    {
        /// <summary>
        /// Option identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Option name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// Option name in English
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Number of items with this option
        /// </summary>
        public int Count { get; set; }
    }
}
