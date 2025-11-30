using Common.Enumerations.Review;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbProductReview : BaseEntity
    {
        public int ReviewNumber { get; set; }
        public Guid ProductID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? OrderItemID { get; set; }
        public decimal Rating { get; set; }
        public string ReviewTitle { get; set; } = null!;
        public string ReviewText { get; set; } = null!;
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public int NotHelpfulCount { get; set; }

        // New properties
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
        public bool IsEdited { get; set; }

        // Navigation Properties
        public virtual ICollection<TbReviewVote> ReviewVotes { get; set; } = new List<TbReviewVote>();
        public virtual ICollection<TbReviewReport> ReviewReports { get; set; } = new List<TbReviewReport>();
    }
}
