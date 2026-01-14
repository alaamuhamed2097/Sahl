namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// DTO for creating a vendor promo code participation request
    /// </summary>
    public class CreateVendorPromoCodeParticipationRequestDto
    {
        /// <summary>
        /// The ID of the promo code the vendor wants to participate in
        /// </summary>
        public Guid PromoCodeId { get; set; }

        /// <summary>
        /// Description of the participation (optional) - English
        /// </summary>
        public string? DescriptionEn { get; set; }

        /// <summary>
        /// Description of the participation (optional) - Arabic
        /// </summary>
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// Any additional notes or comments
        /// </summary>
        public string? Notes { get; set; }
    }
}
