using Common.Enumerations.Order;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Returns;
using Domains.Entities.Order.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Refund
{
    public class TbRefund : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Number { get; set; } = null!;

        // Order References
        [Required]
        public Guid OrderDetailId { get; set; }

        // Customer and Vendor References
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid DeliveryAddressId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        // Refund Reason
        [Required]
        public RefundReason RefundReason { get; set; }

        [StringLength(1000)]
        public string? RefundReasonDetails { get; set; }

        public string? RejectionReason { get; set; } // Why request was denied
        public RefundStatus RefundStatus { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReturnShippingCost { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; } = 0m;
        public int RequestedItemsCount { get; set; }
        public int ApprovedItemsCount { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ReturnTrackingNumber { get; set; } 
        public DateTime RequestDateUTC { get; set; }
        public DateTime? ApprovedDateUTC { get; set; }
        public DateTime? ReturnedDateUTC { get; set; }
        public DateTime? RefundedDateUTC { get; set; }

        [MaxLength(1000)]
        public string? AdminNotes { get; set; }
       
        [StringLength(450)]
        public string? AdminUserId { get; set; }

        // Navigation Properties
        [ForeignKey("OrderDetailId")]
        public virtual TbOrderDetail OrderDetail { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public virtual TbCustomer Customer { get; set; } = null!;

        [ForeignKey("DeliveryAddressId")]
        public virtual TbCustomerAddress? CustomerAddress { get; set; }

        [ForeignKey("VendorId")]
        public virtual TbVendor Vendor { get; set; } = null!;

        [ForeignKey("AdminUserId")]
        public virtual ApplicationUser? AdminUser { get; set; }

        public virtual ICollection<TbRefundItemVideo> RefundItemVideos { get; set; } = new List<TbRefundItemVideo>();
        public virtual ICollection<TbRefundStatusHistory> RefundStatusHistories { get; set; } = new List<TbRefundStatusHistory>();
    }
}
