using Common.Enumerations.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Customer
{
	public class OrderHistoryDto
	{
		public Guid Id { get; set; }
		public string OrderNumber { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public OrderProgressStatus Status { get; set; }
		public int ItemCount { get; set; }
		
	}
}
