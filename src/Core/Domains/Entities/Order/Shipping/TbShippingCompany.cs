using Domains.Entities.ECommerceSystem.Review;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Order.Shipping
{
    public class TbShippingCompany : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? LogoImagePath { get; set; }

        [Required]
        [MaxLength(10)]
        public string PhoneCode { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Website { get; set; }

        public bool IsActive { get; set; } = true;

        // API integration details for tracking
        [MaxLength(200)]
        public string? TrackingApiEndpoint { get; set; }

        [MaxLength(100)]
        public string? ApiKey { get; set; }

        // Average delivery time in days
        public int? AverageDeliveryDays { get; set; }

        // Navigation Properties
        public virtual ICollection<TbOrderShipment> Shipments { get; set; } = new HashSet<TbOrderShipment>();
        public virtual ICollection<TbShippingCompanyReview> ShippingCompanyReviews { get; set; } = new HashSet<TbShippingCompanyReview>();
    }
}