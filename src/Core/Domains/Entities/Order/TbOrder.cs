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
    /// Main order entity with complete pricing breakdown and payment summary
    /// </summary>
    public class TbOrder : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Number { get; set; } = null!;

        // ==================== PRICING BREAKDOWN ====================

        // Sum of all order details subtotals
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        // DiscountAmount from coupon (distributed across shipments)
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;

        // Total shipping amount (sum of all TbOrderShipment.ShippingCost)
        // This is CALCULATED from shipments
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingAmount { get; set; } = 0m;

        // Tax amount (distributed across shipments)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; } = 0m;

        // Final total price (SubTotal + ShippingAmount + TaxAmount - DiscountAmount)
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // ==================== PAYMENT BREAKDOWN SUMMARY ====================

        // Amount paid via Wallet
        // Calculated from: SUM(TbOrderPayment.Amount WHERE PaymentMethodType = Wallet AND PaymentStatus = Completed)
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletPaidAmount { get; set; } = 0m;

        // Amount paid via Credit/Debit Card
        // Calculated from: SUM(TbOrderPayment.Amount WHERE PaymentMethodType = Card AND PaymentStatus = Completed)
        [Column(TypeName = "decimal(18,2)")]
        public decimal CardPaidAmount { get; set; } = 0m;

        // Amount paid via Cash on Delivery
        // Calculated from: SUM(TbShipmentPayment.Amount WHERE PaymentStatus = Completed)
        [Column(TypeName = "decimal(18,2)")]
        public decimal CashPaidAmount { get; set; } = 0m;

        // Total amount actually paid
        // Formula: WalletPaidAmount + CardPaidAmount + CashPaidAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPaidAmount { get; set; } = 0m;

        // ==================== ADDRESS ====================

        // Address is stored as a reference to customer's saved address
        [ForeignKey("CustomerAddress")]
        public Guid DeliveryAddressId { get; set; }

        // ==================== STATUS ====================

        // Overall payment status
        public PaymentStatus PaymentStatus { get; set; }

        // Order progress status
        public OrderProgressStatus OrderStatus { get; set; } = OrderProgressStatus.Pending;

        // ==================== DATES ====================

        // Date when order was fully delivered
        public DateTime? OrderDeliveryDate { get; set; }

        // Date when order was fully paid (last payment date)
        public DateTime? PaidAt { get; set; }

        // ==================== RELATIONSHIPS ====================

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("Coupon")]
        public Guid? CouponId { get; set; }

        // ==================== ADDITIONAL INFO ====================

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