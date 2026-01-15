namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// DTO for listing vendor promo code participation requests
    /// </summary>
    public class VendorPromoCodeParticipationRequestListDto
    {
        /// <summary>
        /// Request ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Promo code value
        /// </summary>
        public string? PromoCodeValue { get; set; }

        /// <summary>
        /// Promo code title
        /// </summary>
        public string? PromoCodeTitle { get; set; }

        /// <summary>
        /// Request status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Status name
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// When the request was submitted
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// When the request was reviewed
        /// </summary>
        public DateTime? ReviewedAt { get; set; }
    }
}
