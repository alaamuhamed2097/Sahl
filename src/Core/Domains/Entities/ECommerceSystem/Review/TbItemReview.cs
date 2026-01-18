using Common.Enumerations.Review;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Customer;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbItemReview : BaseEntity
    {
        public Guid ItemId { get; set; }
        public Guid CustomerId { get; set; }
		public decimal Rating { get; set; }
        public string ReviewTitle { get; set; } = null!;
        public string ReviewText { get; set; } = null!;
        public string ReviewNumber { get; set; } = null!;
        public int HelpfulVoteCount { get; set; }
        public bool IsEdited { get; set; }
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        // Navigation Properties
		public virtual TbItem Item { get; set; } = null!;
		public virtual TbCustomer Customer { get; set; } = null!;

        public virtual ICollection<TbReviewVote> ReviewVotes { get; set; } = new List<TbReviewVote>();
        public virtual ICollection<TbReviewReport> ReviewReports { get; set; } = new List<TbReviewReport>();
	}
}
