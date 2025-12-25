using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Entities.ECommerceSystem.Customer
{
	public class TbCustomer : BaseEntity
	{
		public string? UserId { get; set; }
        //Navigation Properties
        public virtual ICollection<TbCustomerItemView> CustomerItemViews { get; set; }
    }
}
