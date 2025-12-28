using Common.Enumerations.Review;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbItemReview : BaseEntity
    {
        public int ReviewNumber { get; set; }
        public Guid ItemId { get; set; }
        public Guid CustomerId { get; set; }
     
		
		public decimal Rating { get; set; }
        public string ReviewTitle { get; set; } = null!;
        public string ReviewText { get; set; } = null!;
        //public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public int NotHelpfulCount { get; set; }

        // New properties
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
        public bool IsEdited { get; set; }

        // Navigation Properties
        public virtual ICollection<TbReviewVote> ReviewVotes { get; set; } = new List<TbReviewVote>();
        public virtual ICollection<TbReviewReport> ReviewReports { get; set; } = new List<TbReviewReport>();
		
		public virtual TbItem Item { get; set; } = null!;
		
		public virtual TbCustomer Customer { get; set; } = null!;
	}
}
