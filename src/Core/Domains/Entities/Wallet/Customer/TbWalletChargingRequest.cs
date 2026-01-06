using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enumerations.Wallet.Customer;

namespace Domains.Entities.Wallet.Customer
{
    public class TbWalletChargingRequest : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public Guid? PaymentMethodId { get; set; }

        public WalletTransactionStatus Status { get; set; } = WalletTransactionStatus.Pending;

        public string? GatewayTransactionId { get; set; }

        public string? FailureReason { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
