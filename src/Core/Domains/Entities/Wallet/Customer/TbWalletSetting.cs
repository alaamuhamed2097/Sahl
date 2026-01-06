using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet.Customer
{
    public class TbWalletSetting : BaseEntity
    {
        // Charging Limits
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinChargingAmount { get; set; } = 10m; // Default

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxChargingAmount { get; set; } = 10000m; // Default

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxDailyChargingLimit { get; set; } = 50000m; // Default

        // Fees Configuration (Future Proofing)
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChargingFeePercentage { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ChargingFeeFixed { get; set; } = 0m;

        // Switches
        public bool IsChargingEnabled { get; set; } = true;
        public bool IsPaymentEnabled { get; set; } = true;
        public bool IsTransferEnabled { get; set; } = true;
    }
}
