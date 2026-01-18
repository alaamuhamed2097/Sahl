using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	public class CustomerBasicDto
	{
		public Guid? CustomerId { get; set; }
		public string? CustomerName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
