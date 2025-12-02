using Domains.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Payment
{
    public class TbOrderPayment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentGatewayResponse { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? Notes { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
    }
}
