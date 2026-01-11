using Common.Enumerations.Payment;
using Domains.Entities.Currency;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Payment
{
    /// <summary>
    /// Order payment entity with support for multiple payment methods
    /// </summary>
    public class TbOrderPayment : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [ForeignKey("PaymentMethod")]
        public Guid PaymentMethodId { get; set; }

        // PaymentMethod enum for quick access
        public PaymentMethodType PaymentMethodType { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        // GatewayTransactionId for payment gateway tracking
        [MaxLength(200)]
        public string? GatewayTransactionId { get; set; }

        public string? PaymentGatewayResponse { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? RefundedAt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RefundAmount { get; set; }

        // FailureReason for failed payments
        [MaxLength(500)]
        public string? FailureReason { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // CurrentState field (mapped from IsDeleted in BaseEntity via migration)
        public int CurrentState { get; set; } = 1;

        // Navigation Properties
        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbPaymentMethod PaymentMethod { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
    }
}
