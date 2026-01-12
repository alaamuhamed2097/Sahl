using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	public class OrderDetailBasicDto
	{
		public Guid? OrderDetailId { get; set; }
		public string? ProductName { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Price { get; set; }
		public DateTime? OrderDate { get; set; }
	}
}
