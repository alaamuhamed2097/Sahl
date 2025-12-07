using System;
using System.Collections.Generic;

namespace Shared.DTOs.Wallet
{
    // Customer Wallet DTOs
    public class CustomerWalletDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal PendingAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }

    // Vendor Wallet DTOs
    public class VendorWalletDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal HeldAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }

    // Wallet Transaction DTOs
    public class WalletTransactionCreateDto
    {
        public Guid? CustomerWalletId { get; set; }
        public Guid? VendorWalletId { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public Guid? OrderId { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class WalletTransactionDto
    {
        public Guid Id { get; set; }
        public Guid? CustomerWalletId { get; set; }
        public Guid? VendorWalletId { get; set; }
        public string WalletOwner { get; set; }
        public int TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionStatusName { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string ProcessedByUserName { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    // Platform Treasury DTOs
    public class PlatformTreasuryDto
    {
        public Guid Id { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommissions { get; set; }
        public decimal TotalRefunds { get; set; }
        public decimal PendingPayouts { get; set; }
        public decimal TotalPayouts { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    public class TreasuryTransactionDto
    {
        public Guid Id { get; set; }
        public int TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    // Deposit/Withdrawal Request DTOs
    public class DepositRequestDto
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
    }

    public class WithdrawalRequestDto
    {
        public Guid? CustomerWalletId { get; set; }
        public Guid? VendorWalletId { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountHolderName { get; set; }
        public string Description { get; set; }
    }

    // Search/Filter DTOs
    public class WalletTransactionSearchRequest
    {
        public Guid? CustomerWalletId { get; set; }
        public Guid? VendorWalletId { get; set; }
        public int? TransactionType { get; set; }
        public int? TransactionStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string ReferenceNumber { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class WalletStatisticsDto
    {
        public decimal TotalCustomerWalletBalance { get; set; }
        public decimal TotalVendorWalletBalance { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public decimal TotalTransactionsToday { get; set; }
        public decimal TotalDepositsToday { get; set; }
        public decimal TotalWithdrawalsToday { get; set; }
        public int ActiveCustomerWallets { get; set; }
        public int ActiveVendorWallets { get; set; }
    }
}
