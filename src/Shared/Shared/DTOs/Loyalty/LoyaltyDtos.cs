using System;
using System.Collections.Generic;

namespace Shared.DTOs.Loyalty
{
    // Request DTOs
    public class LoyaltyTierCreateDto
    {
        public string TierCode { get; set; }
        public string TierNameEn { get; set; }
        public string TierNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int TierLevel { get; set; }
        public decimal MinimumPoints { get; set; }
        public decimal? MaximumPoints { get; set; }
        public decimal PointsMultiplier { get; set; }
        public decimal? BirthdayBonusPoints { get; set; }
        public int? FreeShippingThreshold { get; set; }
        public decimal? CashbackPercentage { get; set; }
        public int? PrioritySupport { get; set; }
        public int? ExclusiveDealsAccess { get; set; }
        public string BadgeColor { get; set; }
        public string BadgeIconPath { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class LoyaltyTierUpdateDto : LoyaltyTierCreateDto
    {
        public Guid Id { get; set; }
    }

    // Response DTOs
    public class LoyaltyTierDto
    {
        public Guid Id { get; set; }
        public string TierCode { get; set; }
        public string TierNameEn { get; set; }
        public string TierNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int TierLevel { get; set; }
        public decimal MinimumPoints { get; set; }
        public decimal? MaximumPoints { get; set; }
        public decimal PointsMultiplier { get; set; }
        public decimal? BirthdayBonusPoints { get; set; }
        public int? FreeShippingThreshold { get; set; }
        public decimal? CashbackPercentage { get; set; }
        public int? PrioritySupport { get; set; }
        public int? ExclusiveDealsAccess { get; set; }
        public string BadgeColor { get; set; }
        public string BadgeIconPath { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int CurrentCustomersCount { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }

    public class CustomerLoyaltyDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public Guid LoyaltyTierId { get; set; }
        public string TierName { get; set; }
        public string TierCode { get; set; }
        public string TierBadgeColor { get; set; }
        public string TierBadgeIcon { get; set; }
        public decimal CurrentPoints { get; set; }
        public decimal TotalPointsEarned { get; set; }
        public decimal TotalPointsRedeemed { get; set; }
        public decimal PointsToNextTier { get; set; }
        public DateTime? TierAchievedDate { get; set; }
        public DateTime? TierExpiryDate { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    public class LoyaltyPointsTransactionCreateDto
    {
        public string UserId { get; set; }
        public int TransactionType { get; set; }
        public decimal Points { get; set; }
        public Guid? OrderId { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class LoyaltyPointsTransactionDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public int TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public decimal Points { get; set; }
        public decimal BalanceAfter { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    // List/Search DTOs
    public class LoyaltyTierListDto
    {
        public List<LoyaltyTierDto> Tiers { get; set; }
        public int TotalCount { get; set; }
    }

    public class CustomerLoyaltySearchRequest
    {
        public Guid? LoyaltyTierId { get; set; }
        public decimal? MinPoints { get; set; }
        public decimal? MaxPoints { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class LoyaltyPointsTransactionSearchRequest
    {
        public string UserId { get; set; }
        public int? TransactionType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
