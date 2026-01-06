using Common.Enumerations.Wallet.Customer;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Wallet.Customer
{
    public class CustomerWalletTransactionsDto
    {
        public DateTime Date { get; set; }
        public WalletTransactionType TransactionType { get; set; }
        public Guid MarketerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public decimal AvailableBalanace { get; set; }
        public decimal PendingBalanace { get; set; }
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal FeeAmount { get; set; }
        public WalletTransactionStatus TransactionStatus { get; set; }

        [JsonIgnore]
        public string CreatedDateLocalFormatted =>
        TimeZoneInfo.ConvertTimeFromUtc(Date, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");
    }
}
