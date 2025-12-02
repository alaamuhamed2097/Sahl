using Domains.Entities.Location;
using Domains.Entities.ECommerceSystem.Customer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem
{
    public class TbCustomerAddress : BaseEntity
    {
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public int AddressType { get; set; }

        [Required]
        [MaxLength(100)]
        public string RecipientName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(4)]
        public string PhoneCode { get; set; } = null!;

        public Guid CityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string District { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = null!;

        [MaxLength(50)]
        public string? BuildingNumber { get; set; }
        [MaxLength(50)]
        public string? Floor { get; set; }
        [MaxLength(50)]
        public string? Apartment { get; set; }
        [MaxLength(200)]
        public string? Landmark { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }

        public virtual TbCustomer Customer { get; set; } = null!;
    }
}
