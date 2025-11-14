using Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domins.Entities.Customer
{
	public class TbCustomer : BaseEntity
	{
		public string? UserId { get; set; }
		public string? Notes { get; set; }

		}
}
