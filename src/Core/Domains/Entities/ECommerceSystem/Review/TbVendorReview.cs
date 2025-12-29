using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Vendor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Entities.ECommerceSystem.Review
{
	public class TbVendorReview : BaseEntity
	{
		public Guid CustomerId { get; set; }
		public Guid VendorId { get; set; }
		public Guid? OrderDetailId { get; set; }
		public decimal Rating { get; set; }
		public string? ReviewText { get; set; }
		public bool IsEdited { get; set; }
		public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

		public bool IsVerifiedPurchase { get; set; }

		public virtual TbCustomer Customer { get; set; } = null!;
		public virtual TbVendor Vendor { get; set; } = null!;
		public virtual TbOrderDetail? OrderDetail { get; set; }
	}

}
