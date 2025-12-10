using Shared.DTOs.Base;
using Common.Enumerations.Review;

namespace Shared.DTOs.Review
{
    public class OfferReviewDto : BaseDto
    {
        public int ReviewNumber { get; set; }
        public Guid OfferID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? OrderItemID { get; set; }
        public decimal Rating { get; set; }
        public string ReviewTitle { get; set; } = null!;
        public string ReviewText { get; set; } = null!;
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public int NotHelpfulCount { get; set; }
        public ReviewStatus Status { get; set; }
        public bool IsEdited { get; set; }
    }
}
