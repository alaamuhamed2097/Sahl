using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Entities.Customer
{
	public class TbCustomer : BaseEntity
	{
		public string? UserId { get; set; }
		public string? Notes { get; set; }
	public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

	public string Email { get; set; } = null!;

	

	}
}
