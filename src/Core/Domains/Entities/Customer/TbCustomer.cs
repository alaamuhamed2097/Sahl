using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Entities.Customer
{
	public class TbCustomer : BaseEntity
	{
		public string? UserId { get; set; }
		public string? Notes { get; set; }

	}
}
