using Shared.DTOs.Base;

namespace Shared.DTOs.Review
{
    public class ProductReviewDto : BaseDto
    {
        public int ReviewNumber { get; set; }
        public Guid ProductID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? OrderItemID { get; set; }
        public decimal? Rating { get; set; }
        public string ReviewTitle { get; set; } = null!;
        public string ReviewText { get; set; } = null!;
        public string? Images { get; set; }
        public string? Videos { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public int NotHelpfulCount { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; } = null!;
        public string? ModerationNotes { get; set; }
        public string? VendorResponse { get; set; }
        public DateTime? VendorResponseDate { get; set; }
    }
}
