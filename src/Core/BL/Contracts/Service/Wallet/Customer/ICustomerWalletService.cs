using Domains.Entities.Wallet.Customer;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Wallet.Customer;

namespace BL.Contracts.Service.Wallet.Customer
{
    public interface ICustomerWalletService
    {
        // Basic Info
        Task<decimal> GetBalanceAsync(string userId);
        Task<TbCustomerWallet> GetWalletAsync(string userId);

        // IN Transactions
        Task<bool> ProcessRefundAsync(string userId, decimal amount, Guid refundId);
        Task<bool> ProcessChargingAsync(string userId, decimal amount, Guid chargingRequestId);

        // OUT Transactions
        Task<bool> PayOrderAsync(string userId, decimal amount, Guid orderId); // Can return a Result object for more details

        // Charging Flow
        Task<PaymentResultDto> InitiateChargingRequestAsync(WalletChargingRequestDto request, string userId);
        Task<bool> VerifyChargingPaymentAsync(string gatewayTransactionId, bool isSuccess, string? failureReason);

        // Reporting
        Task<IEnumerable<CustomerWalletTransactionsDto>> GetTransactionsAsync(string userId);
    }
}
