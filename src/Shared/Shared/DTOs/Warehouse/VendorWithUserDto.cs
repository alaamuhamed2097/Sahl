using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Warehouse
{
	public class VendorWithUserDto
	{
		
		public Guid VendorId { get; set; }
		public Guid UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}
}
