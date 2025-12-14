using Common.Enumerations.Loyalty;
using DAL.ApplicationContext;
using Domains.Entities.Loyalty;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Loyalty;

namespace BL.Services.Loyalty
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly ApplicationDbContext _context;

        public LoyaltyService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Loyalty Tier Management

        public async Task<LoyaltyTierDto> GetLoyaltyTierByIdAsync(Guid id)
        {
            var tier = await _context.TbLoyaltyTiers
                .Include(t => t.CustomerLoyalties)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tier == null) return null;

            return MapToDto(tier);
        }

        public async Task<LoyaltyTierDto> GetLoyaltyTierByCodeAsync(string tierCode)
        {
            var tier = await _context.TbLoyaltyTiers
                .FirstOrDefaultAsync(t => t.TierNameEn == tierCode || t.TierNameAr == tierCode);

            if (tier == null) return null;

            return MapToDto(tier);
        }

        public async Task<List<LoyaltyTierDto>> GetAllLoyaltyTiersAsync()
        {
            var tiers = await _context.TbLoyaltyTiers
                .Include(t => t.CustomerLoyalties)
                .OrderBy(t => t.DisplayOrder)
                .ToListAsync();

            return tiers.Select(MapToDto).ToList();
        }

        public async Task<List<LoyaltyTierDto>> GetActiveLoyaltyTiersAsync()
        {
            var tiers = await _context.TbLoyaltyTiers
                .Include(t => t.CustomerLoyalties)
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.DisplayOrder)
                .ToListAsync();

            return tiers.Select(MapToDto).ToList();
        }

        public async Task<LoyaltyTierDto> CreateLoyaltyTierAsync(LoyaltyTierCreateDto dto)
        {
            var tier = new TbLoyaltyTier
            {
                TierNameEn = dto.TierNameEn,
                TierNameAr = dto.TierNameAr,
                DescriptionEn = dto.DescriptionEn,
                DescriptionAr = dto.DescriptionAr,
                MinimumOrdersPerYear = (int)dto.MinimumPoints,
                MaximumOrdersPerYear = (int)(dto.MaximumPoints ?? 999999),
                PointsMultiplier = dto.PointsMultiplier,
                CashbackPercentage = dto.CashbackPercentage ?? 0,
                HasFreeShipping = dto.FreeShippingThreshold.HasValue,
                HasPrioritySupport = dto.PrioritySupport.HasValue,
                BadgeColor = dto.BadgeColor,
                BadgeIconPath = dto.BadgeIconPath,
                DisplayOrder = dto.DisplayOrder,
                IsDeleted = !dto.IsActive
            };

            _context.TbLoyaltyTiers.Add(tier);
            await _context.SaveChangesAsync();

            return MapToDto(tier);
        }

        public async Task<LoyaltyTierDto> UpdateLoyaltyTierAsync(LoyaltyTierUpdateDto dto)
        {
            var tier = await _context.TbLoyaltyTiers.FindAsync(dto.Id);
            if (tier == null) throw new Exception("Loyalty tier not found");

            tier.TierNameEn = dto.TierNameEn;
            tier.TierNameAr = dto.TierNameAr;
            tier.DescriptionEn = dto.DescriptionEn;
            tier.DescriptionAr = dto.DescriptionAr;
            tier.MinimumOrdersPerYear = (int)dto.MinimumPoints;
            tier.MaximumOrdersPerYear = (int)(dto.MaximumPoints ?? 999999);
            tier.PointsMultiplier = dto.PointsMultiplier;
            tier.CashbackPercentage = dto.CashbackPercentage ?? 0;
            tier.HasFreeShipping = dto.FreeShippingThreshold.HasValue;
            tier.HasPrioritySupport = dto.PrioritySupport.HasValue;
            tier.BadgeColor = dto.BadgeColor;
            tier.BadgeIconPath = dto.BadgeIconPath;
            tier.DisplayOrder = dto.DisplayOrder;
            tier.IsDeleted = !dto.IsActive;
            tier.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(tier);
        }

        public async Task<bool> DeleteLoyaltyTierAsync(Guid id)
        {
            var tier = await _context.TbLoyaltyTiers.FindAsync(id);
            if (tier == null) return false;

            var hasCustomers = await _context.TbCustomerLoyalties.AnyAsync(cl => cl.LoyaltyTierId == id);
            if (hasCustomers)
                throw new Exception("Cannot delete tier with active customers");

            _context.TbLoyaltyTiers.Remove(tier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateLoyaltyTierAsync(Guid id)
        {
            var tier = await _context.TbLoyaltyTiers.FindAsync(id);
            if (tier == null) return false;

            tier.IsDeleted = false;
            tier.UpdatedDateUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateLoyaltyTierAsync(Guid id)
        {
            var tier = await _context.TbLoyaltyTiers.FindAsync(id);
            if (tier == null) return false;

            tier.IsDeleted = true;
            tier.UpdatedDateUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Customer Loyalty Management

        public async Task<CustomerLoyaltyDto> GetCustomerLoyaltyAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .Include(cl => cl.User)
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return null;

            return MapToCustomerLoyaltyDto(loyalty);
        }

        public async Task<CustomerLoyaltyDto> GetCustomerLoyaltyByIdAsync(Guid id)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .Include(cl => cl.User)
                .FirstOrDefaultAsync(cl => cl.Id == id);

            if (loyalty == null) return null;

            return MapToCustomerLoyaltyDto(loyalty);
        }

        public async Task<List<CustomerLoyaltyDto>> GetCustomerLoyaltiesByTierAsync(Guid tierId)
        {
            var loyalties = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .Include(cl => cl.User)
                .Where(cl => cl.LoyaltyTierId == tierId)
                .ToListAsync();

            return loyalties.Select(MapToCustomerLoyaltyDto).ToList();
        }

        public async Task<CustomerLoyaltyDto> CreateCustomerLoyaltyAsync(Guid customerId)
        {
            var lowestTier = await _context.TbLoyaltyTiers
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.MinimumOrdersPerYear)
                .FirstOrDefaultAsync();

            if (lowestTier == null)
                throw new Exception("No active loyalty tiers found");

            var loyalty = new TbCustomerLoyalty
            {
                UserId = customerId.ToString(),
                LoyaltyTierId = lowestTier.Id,
                TotalPoints = 0,
                AvailablePoints = 0,
                UsedPoints = 0,
                ExpiredPoints = 0,
                TotalOrdersThisYear = 0,
                TotalSpentThisYear = 0,
                LastTierUpgradeDate = DateTime.UtcNow
            };

            _context.TbCustomerLoyalties.Add(loyalty);
            await _context.SaveChangesAsync();

            return await GetCustomerLoyaltyAsync(customerId);
        }

        public async Task<bool> UpdateCustomerTierAsync(Guid customerId, Guid newTierId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return false;

            loyalty.LoyaltyTierId = newTierId;
            loyalty.LastTierUpgradeDate = DateTime.UtcNow;
            loyalty.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecalculateCustomerTierAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return false;

            var appropriateTier = await _context.TbLoyaltyTiers
                .Where(t => !t.IsDeleted && t.MinimumOrdersPerYear <= loyalty.TotalOrdersThisYear)
                .Where(t => t.MaximumOrdersPerYear >= loyalty.TotalOrdersThisYear)
                .OrderByDescending(t => t.MinimumOrdersPerYear)
                .FirstOrDefaultAsync();

            if (appropriateTier != null && appropriateTier.Id != loyalty.LoyaltyTierId)
            {
                loyalty.LoyaltyTierId = appropriateTier.Id;
                loyalty.LastTierUpgradeDate = DateTime.UtcNow;
                loyalty.UpdatedDateUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        #endregion

        #region Loyalty Points Management

        public async Task<LoyaltyPointsTransactionDto> AddPointsAsync(LoyaltyPointsTransactionCreateDto dto)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserId == dto.UserId);

            if (loyalty == null)
                throw new Exception("Customer loyalty not found");

            var transaction = new TbLoyaltyPointsTransaction
            {
                CustomerLoyaltyId = loyalty.Id,
                TransactionType = (PointsTransactionType)dto.TransactionType,
                Points = dto.Points,
                OrderId = dto.OrderId,
                DescriptionEn = dto.Description ?? "Points earned",
                DescriptionAr = dto.Description ?? "???? ??????",
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            loyalty.TotalPoints += dto.Points;
            loyalty.AvailablePoints += dto.Points;
            loyalty.UpdatedDateUtc = DateTime.UtcNow;

            _context.TbLoyaltyPointsTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            if (Guid.TryParse(dto.UserId, out var customerId))
                await RecalculateCustomerTierAsync(customerId);

            return MapToTransactionDto(transaction);
        }

        public async Task<LoyaltyPointsTransactionDto> RedeemPointsAsync(Guid customerId, decimal points, string description)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null)
                throw new Exception("Customer loyalty not found");

            if (loyalty.AvailablePoints < points)
                throw new Exception("Insufficient points balance");

            var transaction = new TbLoyaltyPointsTransaction
            {
                CustomerLoyaltyId = loyalty.Id,
                TransactionType = PointsTransactionType.RedeemedForDiscount,
                Points = -points,
                DescriptionEn = description ?? "Points redeemed",
                DescriptionAr = description ?? "???? ??????"
            };

            loyalty.AvailablePoints -= points;
            loyalty.UsedPoints += points;
            loyalty.UpdatedDateUtc = DateTime.UtcNow;

            _context.TbLoyaltyPointsTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return MapToTransactionDto(transaction);
        }

        public async Task<decimal> GetCustomerPointsBalanceAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            return loyalty?.AvailablePoints ?? 0;
        }

        public async Task<List<LoyaltyPointsTransactionDto>> GetCustomerTransactionsAsync(Guid customerId, int pageNumber = 1, int pageSize = 20)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return new List<LoyaltyPointsTransactionDto>();

            var transactions = await _context.TbLoyaltyPointsTransactions
                .Where(t => t.CustomerLoyaltyId == loyalty.Id)
                .OrderByDescending(t => t.CreatedDateUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return transactions.Select(MapToTransactionDto).ToList();
        }

        public async Task<List<LoyaltyPointsTransactionDto>> SearchTransactionsAsync(LoyaltyPointsTransactionSearchRequest request)
        {
            var query = _context.TbLoyaltyPointsTransactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                var loyalty = await _context.TbCustomerLoyalties
                    .FirstOrDefaultAsync(cl => cl.UserId == request.UserId);

                if (loyalty != null)
                    query = query.Where(t => t.CustomerLoyaltyId == loyalty.Id);
            }

            if (request.TransactionType.HasValue)
                query = query.Where(t => (int)t.TransactionType == request.TransactionType);

            if (request.FromDate.HasValue)
                query = query.Where(t => t.CreatedDateUtc >= request.FromDate);

            if (request.ToDate.HasValue)
                query = query.Where(t => t.CreatedDateUtc <= request.ToDate);

            var transactions = await query
                .OrderByDescending(t => t.CreatedDateUtc)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return transactions.Select(MapToTransactionDto).ToList();
        }

        #endregion

        #region Loyalty Analytics

        public async Task<decimal> CalculatePointsForOrderAsync(Guid orderId)
        {
            return 0;
        }

        public async Task<bool> AwardOrderPointsAsync(Guid orderId)
        {
            return false;
        }

        public async Task<bool> AwardBirthdayBonusAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null)
                return false;

            var bonusPoints = 100m; // Default birthday bonus

            await AddPointsAsync(new LoyaltyPointsTransactionCreateDto
            {
                UserId = customerId.ToString(),
                TransactionType = (int)PointsTransactionType.Bonus,
                Points = bonusPoints,
                Description = "Birthday Bonus Points"
            });

            return true;
        }

        public async Task<LoyaltyTierDto> GetNextTierForCustomerAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return null;

            var nextTier = await _context.TbLoyaltyTiers
                .Where(t => !t.IsDeleted && t.MinimumOrdersPerYear > loyalty.LoyaltyTier.MinimumOrdersPerYear)
                .OrderBy(t => t.MinimumOrdersPerYear)
                .FirstOrDefaultAsync();

            return nextTier != null ? MapToDto(nextTier) : null;
        }

        public async Task<decimal> CalculatePointsToNextTierAsync(Guid customerId)
        {
            var loyalty = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .FirstOrDefaultAsync(cl => cl.UserId == customerId.ToString());

            if (loyalty == null) return 0;

            var nextTier = await GetNextTierForCustomerAsync(customerId);
            if (nextTier == null) return 0;

            return Math.Max(0, nextTier.MinimumPoints - loyalty.TotalOrdersThisYear);
        }

        public async Task<Dictionary<string, int>> GetTierDistributionAsync()
        {
            var distribution = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .GroupBy(cl => cl.LoyaltyTier.TierNameEn)
                .Select(g => new { TierName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.TierName, x => x.Count);

            return distribution;
        }

        public async Task<Dictionary<DateTime, decimal>> GetPointsActivityReportAsync(DateTime fromDate, DateTime toDate)
        {
            var activity = await _context.TbLoyaltyPointsTransactions
                .Where(t => t.CreatedDateUtc >= fromDate && t.CreatedDateUtc <= toDate)
                .GroupBy(t => t.CreatedDateUtc.Date)
                .Select(g => new { Date = g.Key, TotalPoints = g.Sum(t => t.Points) })
                .ToDictionaryAsync(x => x.Date, x => x.TotalPoints);

            return activity;
        }

        public async Task<List<CustomerLoyaltyDto>> GetTopLoyaltyCustomersAsync(int count = 10)
        {
            var topCustomers = await _context.TbCustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .Include(cl => cl.User)
                .OrderByDescending(cl => cl.AvailablePoints)
                .Take(count)
                .ToListAsync();

            return topCustomers.Select(MapToCustomerLoyaltyDto).ToList();
        }

        #endregion

        #region Helper Methods

        private LoyaltyTierDto MapToDto(TbLoyaltyTier tier)
        {
            return new LoyaltyTierDto
            {
                Id = tier.Id,
                TierCode = tier.TierNameEn,
                TierNameEn = tier.TierNameEn,
                TierNameAr = tier.TierNameAr,
                DescriptionEn = tier.DescriptionEn,
                DescriptionAr = tier.DescriptionAr,
                TierLevel = tier.MinimumOrdersPerYear,
                MinimumPoints = tier.MinimumOrdersPerYear,
                MaximumPoints = tier.MaximumOrdersPerYear,
                PointsMultiplier = tier.PointsMultiplier,
                CashbackPercentage = tier.CashbackPercentage,
                BadgeColor = tier.BadgeColor,
                BadgeIconPath = tier.BadgeIconPath,
                DisplayOrder = tier.DisplayOrder,
                IsActive = !tier.IsDeleted,
                CurrentCustomersCount = tier.CustomerLoyalties?.Count ?? 0,
                CreatedDateUtc = tier.CreatedDateUtc,
                ModifiedDateUtc = tier.UpdatedDateUtc
            };
        }

        private CustomerLoyaltyDto MapToCustomerLoyaltyDto(TbCustomerLoyalty loyalty)
        {
            var nextTier = _context.TbLoyaltyTiers
                .Where(t => !t.IsDeleted && t.MinimumOrdersPerYear > loyalty.LoyaltyTier.MinimumOrdersPerYear)
                .OrderBy(t => t.MinimumOrdersPerYear)
                .FirstOrDefault();

            var customerName = loyalty.User != null
                ? $"{loyalty.User.FirstName ?? ""} {loyalty.User.LastName ?? ""}".Trim()
                : "N/A";

            if (string.IsNullOrWhiteSpace(customerName))
                customerName = "N/A";

            return new CustomerLoyaltyDto
            {
                Id = loyalty.Id,
                UserId = loyalty.UserId,
                CustomerName = customerName,
                LoyaltyTierId = loyalty.LoyaltyTierId,
                TierName = loyalty.LoyaltyTier?.TierNameEn ?? "N/A",
                TierCode = loyalty.LoyaltyTier?.TierNameEn ?? "N/A",
                TierBadgeColor = loyalty.LoyaltyTier?.BadgeColor,
                TierBadgeIcon = loyalty.LoyaltyTier?.BadgeIconPath,
                CurrentPoints = loyalty.AvailablePoints,
                TotalPointsEarned = loyalty.TotalPoints,
                TotalPointsRedeemed = loyalty.UsedPoints,
                PointsToNextTier = nextTier != null ? Math.Max(0, nextTier.MinimumOrdersPerYear - loyalty.TotalOrdersThisYear) : 0,
                TierAchievedDate = loyalty.LastTierUpgradeDate,
                CreatedDateUtc = loyalty.CreatedDateUtc
            };
        }

        private LoyaltyPointsTransactionDto MapToTransactionDto(TbLoyaltyPointsTransaction transaction)
        {
            return new LoyaltyPointsTransactionDto
            {
                Id = transaction.Id,
                UserId = transaction.CustomerLoyalty?.UserId ?? Guid.Empty.ToString(),
                CustomerName = "N/A",
                TransactionType = (int)transaction.TransactionType,
                TransactionTypeName = transaction.TransactionType.ToString(),
                Points = transaction.Points,
                BalanceAfter = 0,
                OrderId = transaction.OrderId,
                OrderNumber = "N/A",
                Description = transaction.DescriptionEn,
                ReferenceNumber = transaction.Id.ToString().Substring(0, 12),
                TransactionDate = transaction.CreatedDateUtc,
                CreatedDateUtc = transaction.CreatedDateUtc
            };
        }

        #endregion
    }
}
