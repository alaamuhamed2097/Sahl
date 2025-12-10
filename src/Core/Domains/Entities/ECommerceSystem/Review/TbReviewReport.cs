using Common.Enumerations.Review;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbReviewReport : BaseEntity
    {
        public Guid ReportID { get; set; }

        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }

        public string Reason { get; set; } = null!; // ???: abusive, spam, fake
        public string? Details { get; set; }
        public ReviewReportStatus Status { get; set; } = ReviewReportStatus.Pending; // Pending, Reviewed, Resolved

        // Navigation
        public virtual TbOfferReview Review { get; set; } = null!;
    }
}
