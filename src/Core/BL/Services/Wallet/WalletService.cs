using Common.Enumerations.Wallet;
using DAL.ApplicationContext;
using Domains.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Wallet;

namespace BL.Service.Wallet;

public class WalletService : IWalletService
{
    private readonly ApplicationDbContext _context;

    public WalletService(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Customer Wallet Management

    public async Task<CustomerWalletDto> GetCustomerWalletAsync(Guid customerId)
    {
        var wallet = await _context.TbCustomerWallets
            .FirstOrDefaultAsync(w => w.UserId == customerId.ToString());

        if (wallet == null) return null;

        return MapToCustomerWalletDto(wallet);
    }

    public async Task<CustomerWalletDto> GetCustomerWalletByIdAsync(Guid id)
    {
        var wallet = await _context.TbCustomerWallets.FindAsync(id);
        if (wallet == null) return null;

        return MapToCustomerWalletDto(wallet);
    }

    public async Task<List<CustomerWalletDto>> GetAllCustomerWalletsAsync()
    {
        var wallets = await _context.TbCustomerWallets
            .OrderByDescending(w => w.CreatedDateUtc)
            .ToListAsync();

        return wallets.Select(MapToCustomerWalletDto).ToList();
    }

    public async Task<CustomerWalletDto> CreateCustomerWalletAsync(Guid customerId)
    {
        var existingWallet = await _context.TbCustomerWallets
            .FirstOrDefaultAsync(w => w.UserId == customerId.ToString());

        if (existingWallet != null)
            throw new Exception("Customer already has a wallet");

        var wallet = new TbCustomerWallet
        {
            UserId = customerId.ToString(),
            AvailableBalance = 0,
            PendingBalance = 0,
            TotalEarned = 0,
            TotalSpent = 0,
            CurrencyId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Default currency
            IsDeleted = false // Active
        };

        _context.TbCustomerWallets.Add(wallet);
        await _context.SaveChangesAsync();

        return MapToCustomerWalletDto(wallet);
    }

    public async Task<bool> ActivateCustomerWalletAsync(Guid walletId)
    {
        var wallet = await _context.TbCustomerWallets.FindAsync(walletId);
        if (wallet == null) return false;

        wallet.IsDeleted = false;
        wallet.UpdatedDateUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateCustomerWalletAsync(Guid walletId)
    {
        var wallet = await _context.TbCustomerWallets.FindAsync(walletId);
        if (wallet == null) return false;

        wallet.IsDeleted = true;
        wallet.UpdatedDateUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Vendor Wallet Management

    public async Task<VendorWalletDto> GetVendorWalletAsync(Guid vendorId)
    {
        var wallet = await _context.TbVendorWallets
            .FirstOrDefaultAsync(w => w.VendorId == vendorId);

        if (wallet == null) return null;

        return MapToVendorWalletDto(wallet);
    }

    public async Task<VendorWalletDto> GetVendorWalletByIdAsync(Guid id)
    {
        var wallet = await _context.TbVendorWallets.FindAsync(id);
        if (wallet == null) return null;

        return MapToVendorWalletDto(wallet);
    }

    public async Task<List<VendorWalletDto>> GetAllVendorWalletsAsync()
    {
        var wallets = await _context.TbVendorWallets
            .OrderByDescending(w => w.CreatedDateUtc)
            .ToListAsync();

        return wallets.Select(MapToVendorWalletDto).ToList();
    }

    public async Task<VendorWalletDto> CreateVendorWalletAsync(Guid vendorId)
    {
        var existingWallet = await _context.TbVendorWallets
            .FirstOrDefaultAsync(w => w.VendorId == vendorId);

        if (existingWallet != null)
            throw new Exception("Vendor already has a wallet");

        var wallet = new TbVendorWallet
        {
            VendorId = vendorId,
            AvailableBalance = 0,
            PendingBalance = 0,
            TotalEarned = 0,
            TotalWithdrawn = 0,
            TotalCommissionPaid = 0,
            CurrencyId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Default currency
            IsDeleted = false // Active
        };

        _context.TbVendorWallets.Add(wallet);
        await _context.SaveChangesAsync();

        return MapToVendorWalletDto(wallet);
    }

    public async Task<bool> ActivateVendorWalletAsync(Guid walletId)
    {
        var wallet = await _context.TbVendorWallets.FindAsync(walletId);
        if (wallet == null) return false;

        wallet.IsDeleted = false;
        wallet.UpdatedDateUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateVendorWalletAsync(Guid walletId)
    {
        var wallet = await _context.TbVendorWallets.FindAsync(walletId);
        if (wallet == null) return false;

        wallet.IsDeleted = true;
        wallet.UpdatedDateUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Wallet Transactions

    public async Task<WalletTransactionDto> CreateTransactionAsync(WalletTransactionCreateDto dto)
    {
        var transaction = new TbWalletTransaction
        {
            CustomerWalletId = dto.CustomerWalletId,
            VendorWalletId = dto.VendorWalletId,
            TransactionType = (WalletTransactionType)dto.TransactionType,
            Amount = dto.Amount,
            Status = WalletTransactionStatus.Pending,
            OrderId = dto.OrderId,
            DescriptionEn = dto.Description ?? "Transaction",
            DescriptionAr = dto.Description ?? "?????",
            ReferenceNumber = dto.ReferenceNumber ?? GenerateReferenceNumber()
        };

        _context.TbWalletTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return MapToTransactionDto(transaction);
    }

    public async Task<List<WalletTransactionDto>> GetTransactionsAsync(Guid? customerWalletId, Guid? vendorWalletId)
    {
        var query = _context.TbWalletTransactions.AsQueryable();

        if (customerWalletId.HasValue)
            query = query.Where(t => t.CustomerWalletId == customerWalletId);

        if (vendorWalletId.HasValue)
            query = query.Where(t => t.VendorWalletId == vendorWalletId);

        var transactions = await query
            .OrderByDescending(t => t.CreatedDateUtc)
            .ToListAsync();

        return transactions.Select(MapToTransactionDto).ToList();
    }

    public async Task<List<WalletTransactionDto>> SearchTransactionsAsync(WalletTransactionSearchRequest request)
    {
        var query = _context.TbWalletTransactions.AsQueryable();

        if (request.CustomerWalletId.HasValue)
            query = query.Where(t => t.CustomerWalletId == request.CustomerWalletId);

        if (request.VendorWalletId.HasValue)
            query = query.Where(t => t.VendorWalletId == request.VendorWalletId);

        if (request.TransactionType.HasValue)
            query = query.Where(t => (int)t.TransactionType == request.TransactionType);

        if (request.TransactionStatus.HasValue)
            query = query.Where(t => (int)t.Status == request.TransactionStatus);

        if (request.FromDate.HasValue)
            query = query.Where(t => t.CreatedDateUtc >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(t => t.CreatedDateUtc <= request.ToDate);

        if (request.MinAmount.HasValue)
            query = query.Where(t => t.Amount >= request.MinAmount);

        if (request.MaxAmount.HasValue)
            query = query.Where(t => t.Amount <= request.MaxAmount);

        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
            query = query.Where(t => t.ReferenceNumber == request.ReferenceNumber);

        var transactions = await query
            .OrderByDescending(t => t.CreatedDateUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return transactions.Select(MapToTransactionDto).ToList();
    }

    public async Task<WalletTransactionDto> GetTransactionByIdAsync(Guid id)
    {
        var transaction = await _context.TbWalletTransactions.FindAsync(id);
        if (transaction == null) return null;

        return MapToTransactionDto(transaction);
    }

    public async Task<bool> ApproveTransactionAsync(Guid transactionId, Guid approvedByUserId)
    {
        var transaction = await _context.TbWalletTransactions.FindAsync(transactionId);
        if (transaction == null) return false;

        if (transaction.Status != WalletTransactionStatus.Pending)
            throw new Exception("Only pending transactions can be approved");

        transaction.Status = WalletTransactionStatus.Completed;
        transaction.ProcessedDate = DateTime.UtcNow;
        transaction.ProcessedByUserId = approvedByUserId.ToString();

        // Update wallet balances
        if (transaction.CustomerWalletId.HasValue)
        {
            var wallet = await _context.TbCustomerWallets.FindAsync(transaction.CustomerWalletId.Value);
            if (wallet != null)
            {
                transaction.BalanceBefore = wallet.AvailableBalance;
                wallet.AvailableBalance += transaction.Amount;
                transaction.BalanceAfter = wallet.AvailableBalance;

                if (transaction.Amount > 0)
                    wallet.TotalEarned += transaction.Amount;
                else
                    wallet.TotalSpent += Math.Abs(transaction.Amount);

                wallet.PendingBalance = Math.Max(0, wallet.PendingBalance - Math.Abs(transaction.Amount));
                wallet.LastTransactionDate = DateTime.UtcNow;
                wallet.UpdatedDateUtc = DateTime.UtcNow;
            }
        }

        if (transaction.VendorWalletId.HasValue)
        {
            var wallet = await _context.TbVendorWallets.FindAsync(transaction.VendorWalletId.Value);
            if (wallet != null)
            {
                transaction.BalanceBefore = wallet.AvailableBalance;
                wallet.AvailableBalance += transaction.Amount;
                transaction.BalanceAfter = wallet.AvailableBalance;

                if (transaction.Amount > 0)
                    wallet.TotalEarned += transaction.Amount;
                else
                    wallet.TotalWithdrawn += Math.Abs(transaction.Amount);

                wallet.PendingBalance = Math.Max(0, wallet.PendingBalance - Math.Abs(transaction.Amount));
                wallet.LastTransactionDate = DateTime.UtcNow;
                wallet.UpdatedDateUtc = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectTransactionAsync(Guid transactionId, string reason)
    {
        var transaction = await _context.TbWalletTransactions.FindAsync(transactionId);
        if (transaction == null) return false;

        if (transaction.Status != WalletTransactionStatus.Pending)
            throw new Exception("Only pending transactions can be rejected");

        transaction.Status = WalletTransactionStatus.Failed;
        transaction.ProcessedDate = DateTime.UtcNow;
        transaction.Notes = reason;

        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Deposit & Withdrawal Operations

    public async Task<WalletTransactionDto> ProcessDepositAsync(DepositRequestDto dto)
    {
        var wallet = await _context.TbCustomerWallets
            .FirstOrDefaultAsync(w => w.UserId == dto.UserId);

        if (wallet == null)
            throw new Exception("Customer wallet not found");

        if (wallet.IsDeleted)
            throw new Exception("Wallet is not active");

        var transaction = new TbWalletTransaction
        {
            CustomerWalletId = wallet.Id,
            TransactionType = WalletTransactionType.CustomerDeposit,
            Amount = dto.Amount,
            Status = WalletTransactionStatus.Pending,
            DescriptionEn = dto.Description ?? "Wallet deposit",
            DescriptionAr = dto.Description ?? "????? ?? ???????",
            ReferenceNumber = GenerateReferenceNumber()
        };

        wallet.PendingBalance += dto.Amount;
        wallet.UpdatedDateUtc = DateTime.UtcNow;

        _context.TbWalletTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return MapToTransactionDto(transaction);
    }

    public async Task<WalletTransactionDto> ProcessWithdrawalAsync(WithdrawalRequestDto dto)
    {
        TbWalletTransaction transaction;

        if (dto.CustomerWalletId.HasValue)
        {
            var wallet = await _context.TbCustomerWallets.FindAsync(dto.CustomerWalletId.Value);
            if (wallet == null)
                throw new Exception("Customer wallet not found");

            if (wallet.IsDeleted)
                throw new Exception("Wallet is not active");

            if (wallet.AvailableBalance < dto.Amount)
                throw new Exception("Insufficient balance");

            transaction = new TbWalletTransaction
            {
                CustomerWalletId = wallet.Id,
                TransactionType = WalletTransactionType.CustomerWithdrawal,
                Amount = -dto.Amount,
                Status = WalletTransactionStatus.Pending,
                DescriptionEn = dto.Description ?? "Wallet withdrawal",
                DescriptionAr = dto.Description ?? "??? ?? ???????",
                ReferenceNumber = GenerateReferenceNumber()
            };

            wallet.PendingBalance += dto.Amount;
        }
        else if (dto.VendorWalletId.HasValue)
        {
            var wallet = await _context.TbVendorWallets.FindAsync(dto.VendorWalletId.Value);
            if (wallet == null)
                throw new Exception("Vendor wallet not found");

            if (wallet.IsDeleted)
                throw new Exception("Wallet is not active");

            var availableBalance = wallet.AvailableBalance;
            if (availableBalance < dto.Amount)
                throw new Exception("Insufficient available balance");

            transaction = new TbWalletTransaction
            {
                VendorWalletId = wallet.Id,
                TransactionType = WalletTransactionType.VendorWithdrawal,
                Amount = -dto.Amount,
                Status = WalletTransactionStatus.Pending,
                DescriptionEn = dto.Description ?? "Wallet withdrawal",
                DescriptionAr = dto.Description ?? "??? ?? ???????",
                ReferenceNumber = GenerateReferenceNumber()
            };

            wallet.PendingBalance += dto.Amount;
        }
        else
        {
            throw new Exception("Either CustomerWalletId or VendorWalletId must be provided");
        }

        _context.TbWalletTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return MapToTransactionDto(transaction);
    }

    public async Task<decimal> GetCustomerBalanceAsync(Guid customerId)
    {
        var wallet = await _context.TbCustomerWallets
            .FirstOrDefaultAsync(w => w.UserId == customerId.ToString());

        return wallet?.AvailableBalance ?? 0;
    }

    public async Task<decimal> GetVendorBalanceAsync(Guid vendorId)
    {
        var wallet = await _context.TbVendorWallets
            .FirstOrDefaultAsync(w => w.VendorId == vendorId);

        return wallet?.AvailableBalance ?? 0;
    }

    #endregion

    #region Platform Treasury

    public async Task<PlatformTreasuryDto> GetPlatformTreasuryAsync()
    {
        var treasury = await _context.TbPlatformTreasuries
            .OrderByDescending(t => t.UpdatedDateUtc)
            .FirstOrDefaultAsync();

        if (treasury == null)
        {
            treasury = new TbPlatformTreasury
            {
                TotalBalance = 0,
                TotalRevenue = 0,
                TotalCommissions = 0,
                TotalRefunds = 0,
                PendingPayouts = 0,
                TotalPayouts = 0,
                LastUpdatedUtc = DateTime.UtcNow
            };

            _context.TbPlatformTreasuries.Add(treasury);
            await _context.SaveChangesAsync();
        }

        return MapToTreasuryDto(treasury);
    }

    public async Task<bool> UpdatePlatformTreasuryAsync()
    {
        var treasury = await _context.TbPlatformTreasuries
            .OrderByDescending(t => t.LastUpdatedUtc)
            .FirstOrDefaultAsync();

        if (treasury == null) return false;

        var customerWalletTotal = await _context.TbCustomerWallets.SumAsync(w => w.AvailableBalance);
        var vendorWalletTotal = await _context.TbVendorWallets.SumAsync(w => w.AvailableBalance);
        var pendingAmount = await _context.TbWalletTransactions
            .Where(t => t.Status == WalletTransactionStatus.Pending)
            .SumAsync(t => t.Amount);

        treasury.TotalBalance = customerWalletTotal + vendorWalletTotal;
        treasury.PendingPayouts = Math.Abs(pendingAmount);
        treasury.LastUpdatedUtc = DateTime.UtcNow;
        treasury.UpdatedDateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RecordRevenueAsync(decimal amount, string description, Guid? orderId = null)
    {
        var treasury = await GetOrCreateTreasury();

        treasury.TotalRevenue += amount;
        treasury.TotalBalance += amount;
        treasury.LastUpdatedUtc = DateTime.UtcNow;
        treasury.UpdatedDateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RecordCommissionAsync(decimal amount, string description, Guid? orderId = null)
    {
        var treasury = await GetOrCreateTreasury();

        treasury.TotalCommissions += amount;
        treasury.TotalBalance += amount;
        treasury.LastUpdatedUtc = DateTime.UtcNow;
        treasury.UpdatedDateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RecordRefundAsync(decimal amount, string description, Guid? orderId = null)
    {
        var treasury = await GetOrCreateTreasury();

        treasury.TotalRefunds += amount;
        treasury.TotalBalance -= amount;
        treasury.LastUpdatedUtc = DateTime.UtcNow;
        treasury.UpdatedDateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Statistics & Reports

    public async Task<WalletStatisticsDto> GetWalletStatisticsAsync()
    {
        var totalCustomerBalance = await _context.TbCustomerWallets.SumAsync(w => w.AvailableBalance);
        var totalVendorBalance = await _context.TbVendorWallets.SumAsync(w => w.AvailableBalance);
        var totalPending = await _context.TbCustomerWallets.SumAsync(w => w.PendingBalance) +
                          await _context.TbVendorWallets.SumAsync(w => w.PendingBalance);

        var today = DateTime.UtcNow.Date;
        var todayTransactions = await _context.TbWalletTransactions
            .Where(t => t.CreatedDateUtc >= today)
            .ToListAsync();

        var activeCustomerWallets = await _context.TbCustomerWallets.CountAsync(w => !w.IsDeleted);
        var activeVendorWallets = await _context.TbVendorWallets.CountAsync(w => !w.IsDeleted);

        return new WalletStatisticsDto
        {
            TotalCustomerWalletBalance = totalCustomerBalance,
            TotalVendorWalletBalance = totalVendorBalance,
            TotalPendingAmount = totalPending,
            TotalTransactionsToday = todayTransactions.Sum(t => t.Amount),
            TotalDepositsToday = todayTransactions.Where(t => t.Amount > 0).Sum(t => t.Amount),
            TotalWithdrawalsToday = Math.Abs(todayTransactions.Where(t => t.Amount < 0).Sum(t => t.Amount)),
            ActiveCustomerWallets = activeCustomerWallets,
            ActiveVendorWallets = activeVendorWallets
        };
    }

    public async Task<WalletStatisticsDto> GetWalletStatisticsAsync(DateTime fromDate, DateTime toDate)
    {
        var transactions = await _context.TbWalletTransactions
            .Where(t => t.CreatedDateUtc >= fromDate && t.CreatedDateUtc <= toDate)
            .ToListAsync();

        return new WalletStatisticsDto
        {
            TotalTransactionsToday = transactions.Sum(t => t.Amount),
            TotalDepositsToday = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount),
            TotalWithdrawalsToday = Math.Abs(transactions.Where(t => t.Amount < 0).Sum(t => t.Amount))
        };
    }

    public async Task<Dictionary<DateTime, decimal>> GetTransactionSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var transactions = await _context.TbWalletTransactions
            .Where(t => t.CreatedDateUtc >= fromDate && t.CreatedDateUtc <= toDate)
            .GroupBy(t => t.CreatedDateUtc.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(t => t.Amount) })
            .ToDictionaryAsync(x => x.Date, x => x.Total);

        return transactions;
    }

    #endregion

    #region Helper Methods

    private async Task<TbPlatformTreasury> GetOrCreateTreasury()
    {
        var treasury = await _context.TbPlatformTreasuries
            .OrderByDescending(t => t.LastUpdatedUtc)
            .FirstOrDefaultAsync();

        if (treasury == null)
        {
            treasury = new TbPlatformTreasury
            {
                TotalBalance = 0,
                TotalRevenue = 0,
                TotalCommissions = 0,
                TotalRefunds = 0,
                PendingPayouts = 0,
                TotalPayouts = 0,
                LastUpdatedUtc = DateTime.UtcNow
            };

            _context.TbPlatformTreasuries.Add(treasury);
            await _context.SaveChangesAsync();
        }

        return treasury;
    }

    private string GenerateReferenceNumber()
    {
        return $"WTX{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
    }

    private CustomerWalletDto MapToCustomerWalletDto(TbCustomerWallet wallet)
    {
        return new CustomerWalletDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            CustomerName = "N/A", // Would need to join
            Balance = wallet.AvailableBalance,
            TotalDeposits = wallet.TotalEarned,
            TotalWithdrawals = wallet.TotalSpent,
            PendingAmount = wallet.PendingBalance,
            IsActive = !wallet.IsDeleted,
            CreatedDateUtc = wallet.CreatedDateUtc,
            ModifiedDateUtc = wallet.UpdatedDateUtc
        };
    }

    private VendorWalletDto MapToVendorWalletDto(TbVendorWallet wallet)
    {
        return new VendorWalletDto
        {
            Id = wallet.Id,
            VendorId = wallet.VendorId,
            VendorName = "N/A", // Would need to join
            Balance = wallet.AvailableBalance,
            TotalEarnings = wallet.TotalEarned,
            TotalWithdrawals = wallet.TotalWithdrawn,
            PendingAmount = wallet.PendingBalance,
            HeldAmount = 0, // Calculated field if needed
            IsActive = !wallet.IsDeleted,
            CreatedDateUtc = wallet.CreatedDateUtc,
            ModifiedDateUtc = wallet.UpdatedDateUtc
        };
    }

    private WalletTransactionDto MapToTransactionDto(TbWalletTransaction transaction)
    {
        return new WalletTransactionDto
        {
            Id = transaction.Id,
            CustomerWalletId = transaction.CustomerWalletId,
            VendorWalletId = transaction.VendorWalletId,
            WalletOwner = "N/A",
            TransactionType = (int)transaction.TransactionType,
            TransactionTypeName = transaction.TransactionType.ToString(),
            Amount = transaction.Amount,
            BalanceBefore = transaction.BalanceBefore ?? 0m,
            BalanceAfter = transaction.BalanceAfter ?? 0m,
            TransactionStatus = (int)transaction.Status,
            TransactionStatusName = transaction.Status.ToString(),
            OrderId = transaction.OrderId,
            OrderNumber = "N/A",
            Description = transaction.DescriptionEn,
            ReferenceNumber = transaction.ReferenceNumber,
            TransactionDate = transaction.CreatedDateUtc,
            ProcessedDate = transaction.ProcessedDate,
            ProcessedByUserName = transaction.ProcessedByUser?.UserName ?? string.Empty,
            Notes = transaction.Notes,
            CreatedDateUtc = transaction.CreatedDateUtc
        };
    }

    private PlatformTreasuryDto MapToTreasuryDto(TbPlatformTreasury treasury)
    {
        return new PlatformTreasuryDto
        {
            Id = treasury.Id,
            TotalBalance = treasury.TotalBalance,
            TotalRevenue = treasury.TotalRevenue,
            TotalCommissions = treasury.TotalCommissions,
            TotalRefunds = treasury.TotalRefunds,
            PendingPayouts = treasury.PendingPayouts,
            TotalPayouts = treasury.TotalPayouts,
            LastUpdatedUtc = treasury.LastUpdatedUtc,
            CreatedDateUtc = treasury.CreatedDateUtc
        };
    }

    #endregion
}
