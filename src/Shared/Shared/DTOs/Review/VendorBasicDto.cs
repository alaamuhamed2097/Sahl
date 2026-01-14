using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	public class VendorBasicDto
	{
		public Guid? VendorId { get; set; }
		public string? VendorName { get; set; }
		public string? Email { get; set; }
		public string? StoreName { get; set; }
	}
}
