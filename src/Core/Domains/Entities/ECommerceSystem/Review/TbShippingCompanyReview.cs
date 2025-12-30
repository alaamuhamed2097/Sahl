using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order.Shipping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Entities.ECommerceSystem.Review
{
	public class TbShippingCompanyReview : BaseEntity
	{
		public Guid CustomerId { get; set; }         
		public Guid OrderDetailId { get; set; }      
		public Guid ShippingCompanyId { get; set; }  

		public decimal Rating { get; set; }          
		public string? ReviewText { get; set; }
		public bool IsEdited { get; set; }
		public virtual TbCustomer Customer { get; set; } = null!;
		public virtual TbOrderDetail? OrderDetail { get; set; }
		public virtual TbShippingCompany ShippingCompany { get; set; } = null!;


	}
}
