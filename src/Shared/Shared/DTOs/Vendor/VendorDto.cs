using Common.Enumerations.VendorType;
using Resources;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Vendor
{
	public class VendorDto : BaseDto
	{
		public string? UserId { get; set; }

		public VendorType VendorType { get; set; }

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]

		public string? CompanyName { get; set; }
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]

		public string? ContactName { get; set; }
		/// </summary>
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(3, MinimumLength = 3, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
		public string? VendorCode { get; set; }

		
		public string? Address { get; set; }
		/// </summary>
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(3, MinimumLength = 3, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]

		public string? PostalCode { get; set; }

		
		public string? CommercialRegister { get; set; }

		public bool VATRegistered { get; set; }
		public string? TaxNumber { get; set; }

	
		public string? Notes { get; set; }
		public bool IsActive { get; set; }
		public byte? Rating { get; set; }
	}
}
