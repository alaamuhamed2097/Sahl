using Common.Enumerations.Review;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbReviewVote : BaseEntity
    {
        public Guid ItemReviewId { get; set; }
        public Guid CustomerId { get; set; }
        public VoteType VoteType { get; set; }

        // Navigation Properties
        public virtual TbItemReview ItemReview { get; set; } = null!;
    }
}
