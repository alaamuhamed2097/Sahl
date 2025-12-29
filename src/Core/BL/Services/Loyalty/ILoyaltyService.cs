using Shared.DTOs.Loyalty;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Service.Loyalty;

public interface ILoyaltyService
{
    // Loyalty Tier Management
    Task<LoyaltyTierDto> GetLoyaltyTierByIdAsync(Guid id);
    Task<LoyaltyTierDto> GetLoyaltyTierByCodeAsync(string tierCode);
    Task<List<LoyaltyTierDto>> GetAllLoyaltyTiersAsync();
    Task<List<LoyaltyTierDto>> GetActiveLoyaltyTiersAsync();
    Task<LoyaltyTierDto> CreateLoyaltyTierAsync(LoyaltyTierCreateDto dto);
    Task<LoyaltyTierDto> UpdateLoyaltyTierAsync(LoyaltyTierUpdateDto dto);
    Task<bool> DeleteLoyaltyTierAsync(Guid id);
    Task<bool> ActivateLoyaltyTierAsync(Guid id);
    Task<bool> DeactivateLoyaltyTierAsync(Guid id);

    // Customer Loyalty Management
    Task<CustomerLoyaltyDto> GetCustomerLoyaltyAsync(Guid customerId);
    Task<CustomerLoyaltyDto> GetCustomerLoyaltyByIdAsync(Guid id);
    Task<List<CustomerLoyaltyDto>> GetCustomerLoyaltiesByTierAsync(Guid tierId);
    Task<CustomerLoyaltyDto> CreateCustomerLoyaltyAsync(Guid customerId);
    Task<bool> UpdateCustomerTierAsync(Guid customerId, Guid newTierId);
    Task<bool> RecalculateCustomerTierAsync(Guid customerId);

    // Loyalty Points Management
    Task<LoyaltyPointsTransactionDto> AddPointsAsync(LoyaltyPointsTransactionCreateDto dto);
    Task<LoyaltyPointsTransactionDto> RedeemPointsAsync(Guid customerId, decimal points, string description);
    Task<decimal> GetCustomerPointsBalanceAsync(Guid customerId);
    Task<List<LoyaltyPointsTransactionDto>> GetCustomerTransactionsAsync(Guid customerId, int pageNumber = 1, int pageSize = 20);
    Task<List<LoyaltyPointsTransactionDto>> SearchTransactionsAsync(LoyaltyPointsTransactionSearchRequest request);

    // Loyalty Analytics
    Task<decimal> CalculatePointsForOrderAsync(Guid orderId);
    Task<bool> AwardOrderPointsAsync(Guid orderId);
    Task<bool> AwardBirthdayBonusAsync(Guid customerId);
    Task<LoyaltyTierDto> GetNextTierForCustomerAsync(Guid customerId);
    Task<decimal> CalculatePointsToNextTierAsync(Guid customerId);

    // Reports
    Task<Dictionary<string, int>> GetTierDistributionAsync();
    Task<Dictionary<DateTime, decimal>> GetPointsActivityReportAsync(DateTime fromDate, DateTime toDate);
    Task<List<CustomerLoyaltyDto>> GetTopLoyaltyCustomersAsync(int count = 10);
}
