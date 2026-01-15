using System;

namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// DTO for admin listing of vendor promo code participation requests
    /// </summary>
    public class AdminVendorPromoCodeParticipationRequestListDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        public Guid PromoCodeId { get; set; }
        public string? PromoCodeValue { get; set; }
        public string? PromoCodeTitle { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}

