using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Customer;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbReviewReport : BaseEntity
    {
        //public Guid ReportId { get; set; }

        public Guid ItemReviewId { get; set; }
        public Guid CustomerId { get; set; }

        public ReviewReportReason Reason { get; set; }  
        public string? Details { get; set; }
        public ReviewReportStatus Status { get; set; } = ReviewReportStatus.Pending; // Pending, Reviewed, Resolved

        // Navigation
        public virtual TbItemReview ItemReview { get; set; } = null!;
        public virtual TbCustomer Customer { get; set; } = null!;
    }
}
