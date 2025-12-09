using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Loyalty
{
    public class TbCustomerLoyalty : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("LoyaltyTier")]
        public Guid LoyaltyTierId { get; set; }

        [Required]
        public decimal TotalPoints { get; set; } = 0m;

        [Required]
        public decimal AvailablePoints { get; set; } = 0m;

        [Required]
        public decimal UsedPoints { get; set; } = 0m;

        [Required]
        public decimal ExpiredPoints { get; set; } = 0m;

        [Required]
        public int TotalOrdersThisYear { get; set; } = 0;

        [Required]
        public decimal TotalSpentThisYear { get; set; } = 0m;

        public DateTime? LastTierUpgradeDate { get; set; }

        public DateTime? NextTierEligibilityDate { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual TbLoyaltyTier LoyaltyTier { get; set; } = null!;
        public ICollection<TbLoyaltyPointsTransaction> PointsTransactions { get; set; } = new HashSet<TbLoyaltyPointsTransaction>();
    }
}
