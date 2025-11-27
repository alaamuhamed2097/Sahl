using Domains.Entities.CouponCode;
using Domains.Entities.Shipping;
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

        [MaxLength(100)]
        public string Address { get; set; }

        public int PVs { get; set; }

        public int PaymentStatus { get; set; }

        public DateTime? OrderDeliveryDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [ForeignKey("PaymentGatewayMethod")]
        public Guid PaymentGatewayMethodId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; } 

        [ForeignKey("DirectSaleLink")]
        public Guid? DirectSaleLinkId { get; set; }

        [ForeignKey("Coupon")]
        public Guid? CouponId { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompanyId { get; set; }

        public decimal ShippingAmount { get; set; } = 0m;

        public decimal TaxAmount { get; set; } = 0m;


        public virtual ApplicationUser User { get; set; }
        // public virtual TbPaymentGatewayMethod PaymentGatewayMethod { get; set; }
        public virtual TbCouponCode? Coupon { get; set; }
        public virtual TbShippingCompany? ShippingCompany { get; set; }

        public virtual ICollection<TbOrderDetail> OrderDetails { get; set; }
    }
}

