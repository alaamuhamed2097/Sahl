using BL.Contracts.GeneralService;
using Common.Enumerations.Offer;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.ResultModels.DAL.ResultModels;
using DAL.Services;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;
using System.Threading;

namespace DAL.Repositories.Offer
{
    /// <summary>
    /// Implementation of offer repository with transaction support
    /// </summary>
    public class BuyBoxHelperRepository : IBuyBoxHelperRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        public BuyBoxHelperRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        /// <summary>
        /// Recalculate Buy Box winner for a specific item combination
        /// </summary>
        /// <param name="itemCombinationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task RecalculateBuyBoxWinnerByItemCombinationIdAsync(Guid itemCombinationId, CancellationToken cancellationToken = default)
        {
            // Get current user ID
            var userId = Guid.TryParse(
                _currentUserService.GetCurrentUserId(),
                out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            // Get all offers for this combination
            var offerPricings = await _dbContext.TbOfferCombinationPricings
                .Where(p => p.ItemCombinationId == itemCombinationId &&
                            p.StockStatus == StockStatus.InStock &&
                            !p.IsDeleted)
                .Include(ocp => ocp.Offer)
                .ThenInclude(o => o.Vendor)
                .ToListAsync();

            // Check if there are any eligible offers
            if (!offerPricings.Any())
                return;

            // Reset all to non-winner
            foreach (var pricing in offerPricings)
            {
                pricing.IsBuyBoxWinner = false;
            }

            // Buy Box selection
            var winner = offerPricings
               .OrderBy(p => p.Price)
               .ThenByDescending(p => p.Offer?.Vendor?.AverageRating ?? 0)
               .ThenBy(p => p.Offer?.EstimatedDeliveryDays ?? 0)
               .ThenBy(p => p.Offer?.FulfillmentType)
               .ThenByDescending(p => p.AvailableQuantity)
               .ThenBy(p => p.RefundedQuantity)
               .First();

            winner.IsBuyBoxWinner = true;
            winner.UpdatedBy = userId;
            winner.UpdatedDateUtc = DateTime.UtcNow;

            // Save changes
            _dbContext.Entry(winner).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                throw new DataAccessException($"Failed to recalculate Buy Box winner.", _logger);
        }

        /// <summary>
        /// Recalculate Buy Box winners for all combinations of a specific item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task RecalculateBuyBoxWinnersByItemIdAsync(
    Guid itemId,
    CancellationToken cancellationToken = default)
        {
            var userId = Guid.TryParse(
                _currentUserService.GetCurrentUserId(),
                out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            var offerPricings = await _dbContext.TbOfferCombinationPricings
                .Where(p =>
                    p.ItemCombination.ItemId == itemId &&
                    p.StockStatus == StockStatus.InStock &&
                    !p.IsDeleted &&
                    !p.ItemCombination.IsDeleted)
                .Include(p => p.Offer)
                    .ThenInclude(o => o.Vendor)
                .ToListAsync(cancellationToken);

            if (!offerPricings.Any())
                return;

            var groupedByCombination = offerPricings
                .GroupBy(p => p.ItemCombinationId);

            foreach (var group in groupedByCombination)
            {
                // Reset all
                foreach (var pricing in group)
                    pricing.IsBuyBoxWinner = false;

                // Select winner
                var winner = group
                    .OrderBy(p => p.Price)
                    .ThenByDescending(p => p.Offer?.Vendor?.AverageRating ?? 0)
                    .ThenBy(p => p.Offer?.EstimatedDeliveryDays ?? 0)
                    .ThenBy(p => p.Offer?.FulfillmentType)
                    .ThenByDescending(p => p.AvailableQuantity)
                    .ThenBy(p => p.RefundedQuantity)
                    .First();

                winner.IsBuyBoxWinner = true;
                winner.UpdatedBy = userId;
                winner.UpdatedDateUtc = DateTime.UtcNow;
            }

            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                throw new DataAccessException(
                    "Failed to recalculate Buy Box winners.",
                    _logger);
        }
    }
}