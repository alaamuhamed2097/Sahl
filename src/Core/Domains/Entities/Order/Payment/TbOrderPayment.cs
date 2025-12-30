using Common.Enumerations.Payment;
using Domains.Entities.Currency;
using Domains.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Payment
{
    public class TbOrderPayment : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [ForeignKey("PaymentMethod")]
        public Guid PaymentMethodId { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentGatewayResponse { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? Notes { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbPaymentMethod PaymentMethod { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
    }
}
