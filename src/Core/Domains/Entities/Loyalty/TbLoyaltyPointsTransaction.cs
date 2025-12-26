using Common.Enumerations.Loyalty;
using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Loyalty
{
    public class TbLoyaltyPointsTransaction : BaseEntity
    {
        [Required]
        [ForeignKey("CustomerLoyalty")]
        public Guid CustomerLoyaltyId { get; set; }

        [Required]
        public decimal Points { get; set; }

        [Required]
        public PointsTransactionType TransactionType { get; set; }

        [Required]
        [StringLength(200)]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DescriptionAr { get; set; } = string.Empty;

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }

        [ForeignKey("Review")]
        public Guid? ReviewId { get; set; }

        [ForeignKey("Referral")]
        public Guid? ReferralId { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsExpired { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual TbCustomerLoyalty CustomerLoyalty { get; set; } = null!;
        public virtual TbOrder? Order { get; set; }
        public virtual TbItemReview? ItemReview { get; set; }
    }
}
