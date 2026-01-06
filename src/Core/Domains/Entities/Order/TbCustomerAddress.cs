using Domains.Entities.Location;
using Domains.Entities.Order.Refund;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
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

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        public Guid CityId { get; set; }

        public bool IsDefault { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual TbCity City { get; set; } = null!;

        public virtual ICollection<TbOrder> Orders { get; set; } = new HashSet<TbOrder>();
        public virtual ICollection<TbRefund> Refunds { get; set; } = new HashSet<TbRefund>();
    }
}
