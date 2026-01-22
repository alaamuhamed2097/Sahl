using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.User.Customer
{
	public class CustomerUpdateByAdminDto
	{
		[Required]
		public string UserId { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

		[MinLength(6)]
		public string? NewPassword { get; set; }
	}
}
