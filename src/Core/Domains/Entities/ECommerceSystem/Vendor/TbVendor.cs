using Common.Enumerations.IdentificationType;
using Common.Enumerations.VendorStatus;
using Common.Enumerations.VendorType;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Location;
using Domains.Entities.Order.Refund;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Vendor
{
    public class TbVendor : BaseEntity
    {
        public string UserId { get; set; } = null!;

        // Personal Information
        public DateOnly BirthDate { get; set; }

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

        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public Guid CityId { get; set; }

        // Additional Info
        public string? Notes { get; set; }
        public decimal? AverageRating { get; set; }
        public VendorStatus Status { get; set; } = VendorStatus.Pending;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        [ForeignKey("CityId")]
        public virtual TbCity City { get; set; } = null!;

        public virtual ICollection<TbItemReview> ItemReviews { get; set; } = new List<TbItemReview>();
        public virtual ICollection<TbVendorReview> VendorReviews { get; set; } = new List<TbVendorReview>();
        public virtual ICollection<TbRefund> Refunds { get; set; } = new List<TbRefund>();
    }
}

