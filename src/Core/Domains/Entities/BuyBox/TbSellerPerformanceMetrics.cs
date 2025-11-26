using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BuyBox
{
    public class TbSellerPerformanceMetrics : BaseEntity
    {
        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(3,2)")]
        public decimal AverageRating { get; set; } = 0m;

        [Required]
        public int TotalReviews { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal OrderCompletionRate { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal OnTimeShippingRate { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ReturnRate { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal CancellationRate { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ResponseRate { get; set; } = 0m;

        [Required]
        public int AverageResponseTimeInHours { get; set; } = 0;

        [Required]
        public int BuyBoxWins { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal BuyBoxWinRate { get; set; } = 0m;

        [Required]
        public bool UsesFBM { get; set; }

        public DateTime? LastUpdated { get; set; }

        [Required]
        public DateTime CalculatedForPeriodStart { get; set; }

        [Required]
        public DateTime CalculatedForPeriodEnd { get; set; }

        public virtual TbVendor Vendor { get; set; } = null!;
    }
}
