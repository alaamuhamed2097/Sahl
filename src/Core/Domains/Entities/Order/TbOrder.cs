using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Domains.Entities.CouponCode;
using Domains.Entities.ECommerceSystem;
using Domains.Entities.Payment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    public class TbOrder : BaseEntity
    {
        public string Number { get; set; } = null!;

        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? InvoiceId { get; set; }

        // Address is stored as a reference to customer's saved address
        [ForeignKey("CustomerAddress")]
        public Guid DeliveryAddressId { get; set; }

        // PaymentStatus stored as enum
        public PaymentStatus PaymentStatus { get; set; }

        // New OrderStatus column to reflect progress (default 0 -> Pending)
        public OrderProgressStatus OrderStatus { get; set; } = OrderProgressStatus.Pending;

        public DateTime? OrderDeliveryDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Coupon")]
        public Guid? CouponId { get; set; }

        public decimal ShippingAmount { get; set; } = 0m;

        public decimal TaxAmount { get; set; } = 0m;

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual TbCouponCode? Coupon { get; set; }
        public virtual TbCustomerAddress? CustomerAddress { get; set; }
        public virtual ICollection<TbOrderDetail> OrderDetails { get; set; } = new List<TbOrderDetail>();
        public virtual ICollection<TbOrderPayment> OrderPayments { get; set; } = new List<TbOrderPayment>();
    }
}

