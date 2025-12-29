using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.ECommerceSystem.Customer
{
	public class TbCustomer : BaseEntity
	{
		
		public string UserId { get; set; } = null!;
		// Navigation Properties
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; } = null!;


		public virtual ICollection<TbItemReview> ItemReviews { get; set; } = new List<TbItemReview>();
		public virtual ICollection<TbVendorReview> VendorReviews { get; set; } = new List<TbVendorReview>();
		public virtual ICollection<TbShippingCompanyReview> ShippingCompanyReviews { get; set; } = new List<TbShippingCompanyReview>();
		public virtual ICollection<TbReviewReport> ReviewReports { get; set; } = new List<TbReviewReport>();
		public virtual ICollection<TbCustomerItemView> CustomerItemViews { get; set; }


	}
}
