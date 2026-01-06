using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Wallet.Customer
{
    public class WalletChargingRequestDto
    {
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}
