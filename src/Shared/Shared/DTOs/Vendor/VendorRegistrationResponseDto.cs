using System;

namespace Shared.DTOs.Vendor
{
	public class VendorRegistrationResponseDto
	{
		public string UserId { get; set; } = null!;
		public Guid VendorId { get; set; }
		public string FirstName { get; set; } = null!;
		public string? MiddleName { get; set; }
		public string LastName { get; set; } = null!;
		public string? Email { get; set; }
		public string UserName { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
		public string PhoneCode { get; set; } = null!;
		public string? ProfileImagePath { get; set; }
		public string Token { get; set; } = null!;
		public string RefreshToken { get; set; } = null!;
		public DateTime RegisteredDate { get; set; }
		public string StoreName { get; set; } = null!;
	}
}
