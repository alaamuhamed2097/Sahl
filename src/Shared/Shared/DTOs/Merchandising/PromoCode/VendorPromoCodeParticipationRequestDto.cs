namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// DTO for reading vendor promo code participation request details
    /// </summary>
    public class VendorPromoCodeParticipationRequestDto
    {
        /// <summary>
        /// Request ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Related seller request ID (from TbSellerRequest)
        /// </summary>
        public Guid SellerRequestId { get; set; }

        /// <summary>
        /// Vendor ID
        /// </summary>
        public Guid VendorId { get; set; }

        /// <summary>
        /// Vendor store name
        /// </summary>
        public string? VendorName { get; set; }

        /// <summary>
        /// Promo code ID
        /// </summary>
        public Guid PromoCodeId { get; set; }

        /// <summary>
        /// Promo code value
        /// </summary>
        public string? PromoCodeValue { get; set; }

        /// <summary>
        /// Promo code title (in user's language)
        /// </summary>
        public string? PromoCodeTitle { get; set; }

        /// <summary>
        /// Request status (Pending=0, Approved=1, Rejected=2, etc.)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Status name
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// Description - English
        /// </summary>
        public string? DescriptionEn { get; set; }

        /// <summary>
        /// Description - Arabic
        /// </summary>
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Admin notes/feedback on the request
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// When the request was submitted
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// When the request was reviewed
        /// </summary>
        public DateTime? ReviewedAt { get; set; }

        /// <summary>
        /// Name of the user who reviewed the request
        /// </summary>
        public string? ReviewedByUserName { get; set; }

        /// <summary>
        /// When the entity was created
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// When the entity was last updated
        /// </summary>
        public DateTime? UpdatedDateUtc { get; set; }
    }
}
