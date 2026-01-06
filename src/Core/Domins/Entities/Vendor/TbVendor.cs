using Common.Enumerations.IdentificationType;
using Common.Enumerations.VendorType;
using Common.Enumerations.VendorStatus;
using Domains.Entities.Base;
using Domains.Entities.Location;
using Domains.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domins.Entities.Vendor
{
	public class TbVendor : BaseEntity
	{
		// User Relationship
		public string UserId { get; set; } = null!;

		// Personal Information
		public string FirstName { get; set; } = null!;
		public string MiddleName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public DateTime BirthDate { get; set; }

		// Identification Details
		public IdentificationType IdentificationType { get; set; }
		public string IdentificationNumber { get; set; } = null!;
		public string IdentificationImageFrontPath { get; set; } = null!;
		public string IdentificationImageBackPath { get; set; } = null!;

		// Business Information
		public VendorType VendorType { get; set; }
		public string StoreName { get; set; } = null!;
		public bool IsRealEstateRegistered { get; set; }

		// Contact Information
		public string PhoneCode { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;

		// Address Information
		public string Address { get; set; } = null!;
		public Guid CityId { get; set; }
		public string PostalCode { get; set; } = null!;

		// Additional Information
		public string? Notes { get; set; }
		public byte? Rating { get; set; }
		public VendorStatus Status { get; set; } = VendorStatus.Pending;

		// Navigation Properties
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; } = null!;

		[ForeignKey("CityId")]
		public virtual TbCity City { get; set; } = null!;
	}
}
