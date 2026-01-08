using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    /// <summary>
    /// Main order entity with complete pricing breakdown
    /// </summary>
    public class TbOrder : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Number { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        // DiscountAmount from coupon
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; } = 0m;

        // Final total price (SubTotal + Shipping + Tax - Discount)
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? InvoiceId { get; set; }

        // Address is stored as a reference to customer's saved address
        [ForeignKey("CustomerAddress")]
        public Guid DeliveryAddressId { get; set; }

        // PaymentStatus stored as enum
        public PaymentStatus PaymentStatus { get; set; }

        // OrderStatus column to reflect progress (default 0 -> Pending)
        public OrderProgressStatus OrderStatus { get; set; } = OrderProgressStatus.Pending;

        public DateTime? OrderDeliveryDate { get; set; }

        public DateTime? PaidAt { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("Coupon")]
        public Guid? CouponId { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual TbCouponCode? Coupon { get; set; }
        public virtual TbCustomerAddress? CustomerAddress { get; set; }
        public virtual ICollection<TbOrderDetail> OrderDetails { get; set; } = new List<TbOrderDetail>();
        public virtual ICollection<TbOrderPayment> OrderPayments { get; set; } = new List<TbOrderPayment>();
        public virtual ICollection<TbOrderShipment> TbOrderShipments { get; set; } = new List<TbOrderShipment>();
    }
}

