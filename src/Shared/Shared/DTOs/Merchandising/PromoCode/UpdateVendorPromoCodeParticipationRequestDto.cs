namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// DTO for updating a vendor promo code participation request
    /// </summary>
    public class UpdateVendorPromoCodeParticipationRequestDto
    {
        /// <summary>
        /// The ID of the request
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Description of the participation - English
        /// </summary>
        public string? DescriptionEn { get; set; }

        /// <summary>
        /// Description of the participation - Arabic
        /// </summary>
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }
    }
}
