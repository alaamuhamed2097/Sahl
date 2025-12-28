using Common.Enumerations.VendorType;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Vendor
{
	public class VendorDetialsDto
	{
		public string? UserId { get; set; }

		public string? NameAr { get; set; }
		public string? NameEn { get; set; }
		public string? Discription { get; set; }
		public string? Address { get; set; }
		public byte? Rating { get; set; }
	}
}
