using Common.Enumerations.Review;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbReviewVote : BaseEntity
    {
        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }
        public VoteType VoteType { get; set; }

        // Navigation Properties
        public virtual TbProductReview Review { get; set; } = null!;
    }
}
