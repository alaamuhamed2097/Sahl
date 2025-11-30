namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbReviewVote : BaseEntity
    {
        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }
        public string VoteType { get; set; } = null!;
        public string VoteValue { get; set; } = null!;
        public string WithType { get; set; } = null!;

        // Navigation Properties
        public virtual TbProductReview Review { get; set; } = null!;
    }
}
