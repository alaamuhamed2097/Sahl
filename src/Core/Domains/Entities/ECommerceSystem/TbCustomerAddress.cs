using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem
{
    public class TbCustomerAddress : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

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

        public bool IsDefault { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
