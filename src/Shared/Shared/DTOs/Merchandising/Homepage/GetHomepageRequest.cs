namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Request to get homepage blocks
    /// </summary>
    public class GetHomepageRequest
    {
        /// <summary>
        /// User ID for personalization (optional for guests)
        /// </summary>
        public string? UserId { get; set; }
    }
}