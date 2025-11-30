using Shared.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services.Wallet
{
    /// <summary>
    /// Service interface for Wallet and Treasury management
    /// </summary>
    public interface IWalletService
    {
        #region Customer Wallet Management

        /// <summary>
        /// Gets a customer wallet by customer ID
        /// </summary>
        Task<CustomerWalletDto> GetCustomerWalletAsync(Guid customerId);

        /// <summary>
        /// Gets a customer wallet by ID
        /// </summary>
        Task<CustomerWalletDto> GetCustomerWalletByIdAsync(Guid id);

        /// <summary>
        /// Gets all customer wallets
        /// </summary>
        Task<List<CustomerWalletDto>> GetAllCustomerWalletsAsync();

        /// <summary>
        /// Creates a customer wallet
        /// </summary>
        Task<CustomerWalletDto> CreateCustomerWalletAsync(Guid customerId);

        /// <summary>
        /// Activates a customer wallet
        /// </summary>
        Task<bool> ActivateCustomerWalletAsync(Guid walletId);

        /// <summary>
        /// Deactivates a customer wallet
        /// </summary>
        Task<bool> DeactivateCustomerWalletAsync(Guid walletId);

        #endregion

        #region Vendor Wallet Management

        /// <summary>
        /// Gets a vendor wallet by vendor ID
        /// </summary>
        Task<VendorWalletDto> GetVendorWalletAsync(Guid vendorId);

        /// <summary>
        /// Gets a vendor wallet by ID
        /// </summary>
        Task<VendorWalletDto> GetVendorWalletByIdAsync(Guid id);

        /// <summary>
        /// Gets all vendor wallets
        /// </summary>
        Task<List<VendorWalletDto>> GetAllVendorWalletsAsync();

        /// <summary>
        /// Creates a vendor wallet
        /// </summary>
        Task<VendorWalletDto> CreateVendorWalletAsync(Guid vendorId);

        /// <summary>
        /// Activates a vendor wallet
        /// </summary>
        Task<bool> ActivateVendorWalletAsync(Guid walletId);

        /// <summary>
        /// Deactivates a vendor wallet
        /// </summary>
        Task<bool> DeactivateVendorWalletAsync(Guid walletId);

        #endregion

        #region Wallet Transactions

        /// <summary>
        /// Creates a wallet transaction
        /// </summary>
        Task<WalletTransactionDto> CreateTransactionAsync(WalletTransactionCreateDto dto);

        /// <summary>
        /// Gets wallet transactions
        /// </summary>
        Task<List<WalletTransactionDto>> GetTransactionsAsync(Guid? customerWalletId, Guid? vendorWalletId);

        /// <summary>
        /// Searches wallet transactions
        /// </summary>
        Task<List<WalletTransactionDto>> SearchTransactionsAsync(WalletTransactionSearchRequest request);

        /// <summary>
        /// Gets a transaction by ID
        /// </summary>
        Task<WalletTransactionDto> GetTransactionByIdAsync(Guid id);

        /// <summary>
        /// Approves a pending transaction
        /// </summary>
        Task<bool> ApproveTransactionAsync(Guid transactionId, Guid approvedByUserId);

        /// <summary>
        /// Rejects a pending transaction
        /// </summary>
        Task<bool> RejectTransactionAsync(Guid transactionId, string reason);

        #endregion

        #region Deposit & Withdrawal Operations

        /// <summary>
        /// Processes a deposit request
        /// </summary>
        Task<WalletTransactionDto> ProcessDepositAsync(DepositRequestDto dto);

        /// <summary>
        /// Processes a withdrawal request
        /// </summary>
        Task<WalletTransactionDto> ProcessWithdrawalAsync(WithdrawalRequestDto dto);

        /// <summary>
        /// Gets customer wallet balance
        /// </summary>
        Task<decimal> GetCustomerBalanceAsync(Guid customerId);

        /// <summary>
        /// Gets vendor wallet balance
        /// </summary>
        Task<decimal> GetVendorBalanceAsync(Guid vendorId);

        #endregion

        #region Platform Treasury

        /// <summary>
        /// Gets platform treasury information
        /// </summary>
        Task<PlatformTreasuryDto> GetPlatformTreasuryAsync();

        /// <summary>
        /// Updates platform treasury
        /// </summary>
        Task<bool> UpdatePlatformTreasuryAsync();

        /// <summary>
        /// Records revenue transaction
        /// </summary>
        Task<bool> RecordRevenueAsync(decimal amount, string description, Guid? orderId = null);

        /// <summary>
        /// Records commission transaction
        /// </summary>
        Task<bool> RecordCommissionAsync(decimal amount, string description, Guid? orderId = null);

        /// <summary>
        /// Records refund transaction
        /// </summary>
        Task<bool> RecordRefundAsync(decimal amount, string description, Guid? orderId = null);

        #endregion

        #region Statistics & Reports

        /// <summary>
        /// Gets wallet statistics
        /// </summary>
        Task<WalletStatisticsDto> GetWalletStatisticsAsync();

        /// <summary>
        /// Gets wallet statistics by date range
        /// </summary>
        Task<WalletStatisticsDto> GetWalletStatisticsAsync(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets transaction summary for a period
        /// </summary>
        Task<Dictionary<DateTime, decimal>> GetTransactionSummaryAsync(DateTime fromDate, DateTime toDate);

        #endregion
    }
}
