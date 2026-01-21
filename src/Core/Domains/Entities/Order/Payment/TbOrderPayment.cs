using Common.Enumerations.Payment;
using Domains.Entities.Currency;
using Domains.Entities.Wallet.Customer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Payment
{
    /// <summary>
    /// Order payment entity - for ONLINE payments only (Wallet/Card/Mixed)
    /// Supports multiple payment methods for the same order (e.g., Wallet + Card)
    /// For COD, use TbShipmentPayment instead
    /// FINAL VERSION
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

        // Amount paid with this payment method
        // Example: Order total = 1000 EGP
        //   - Payment 1: Wallet = 700 EGP
        //   - Payment 2: Card = 300 EGP
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Internal transaction ID (your system)
        [MaxLength(100)]
        public string? TransactionId { get; set; }

        // ==================== PAYMENT GATEWAY FIELDS ====================

        // Gateway transaction ID (from payment provider: Stripe, PayPal, etc.)
        [MaxLength(200)]
        public string? GatewayTransactionId { get; set; }

        // Full response from payment gateway (JSON format)
        [MaxLength(4000)]
        public string? PaymentGatewayResponse { get; set; }

        // ==================== WALLET SPECIFIC FIELDS ====================

        // If payment method is Wallet, reference to wallet transaction
        [ForeignKey("WalletTransaction")]
        public Guid? WalletTransactionId { get; set; }

        // ==================== DATES ====================

        public DateTime? PaidAt { get; set; }

        public DateTime? RefundedAt { get; set; }

        // ==================== REFUND FIELDS ====================

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RefundAmount { get; set; }

        // Refund transaction ID (if applicable)
        [MaxLength(200)]
        public string? RefundTransactionId { get; set; }

        // ==================== FAILURE TRACKING ====================

        // Reason for failed payments
        [MaxLength(500)]
        public string? FailureReason { get; set; }

        // Number of retry attempts
        public int RetryAttempts { get; set; } = 0;

        // Last retry date
        public DateTime? LastRetryAt { get; set; }

        // ==================== ADDITIONAL INFO ====================

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // IP address from where payment was initiated (for fraud detection)
        [MaxLength(50)]
        public string? IpAddress { get; set; }

        // Navigation Properties
        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbPaymentMethod PaymentMethod { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
        public virtual TbCustomerWalletTransaction? WalletTransaction { get; set; }
    }
}