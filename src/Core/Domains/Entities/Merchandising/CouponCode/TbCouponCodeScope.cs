using Common.Enumerations.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising.CouponCode
{
    /// <summary>
    /// Entity for defining coupon code scope (applicable items/categories)
    /// </summary>
    public class TbCouponCodeScope : BaseEntity
    {
        [Required]
        public Guid CouponCodeId { get; set; }

        [Required]
        public CouponCodeScopeType ScopeType { get; set; }

        [Required]
        public Guid ScopeId { get; set; } // CategoryId or ProductId

        // Navigation Property
        [ForeignKey(nameof(CouponCodeId))]
        public virtual TbCouponCode CouponCode { get; set; }
    }
}