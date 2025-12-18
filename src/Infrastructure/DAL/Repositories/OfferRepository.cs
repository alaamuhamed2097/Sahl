using Common.Enumerations.Offer;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.ResultModels.DAL.ResultModels;
using Domains.Entities.Offer;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    /// <summary>
    /// Implementation of offer repository with transaction support
    /// </summary>
    public class OfferRepository : TableRepository<TbOffer>, IOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly DbSet<TbOfferCombinationPricing> _offerPricing;

        public OfferRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _offerPricing = _dbContext.Set<TbOfferCombinationPricing>();
        }

                /// <summary>
                /// Get offer with all related data
                /// </summary>
                public async Task<VwOffer> GetOfferWithDetailsAsync(Guid offerId, CancellationToken cancellationToken = default)
                {
                    try
                    {
                        var offer = await _dbContext.Set<VwOffer>()
                            .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken);
                        return offer;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error getting offer details for offer ID: {offerId}");
                        throw new DataAccessException($"Failed to retrieve offer details", ex, _logger);
                    }
                }

                /// <summary>
                /// Get offers by item ID
                /// </summary>
                public async Task<IEnumerable<VwOffer>> GetOffersByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default)
                {
                    try
                    {
                        var offers = await _dbContext.Set<VwOffer>()
                            .Where(o => o.ItemId == itemId )
                            .ToListAsync(cancellationToken);

                return offers;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting offers for item ID: {itemId}");
                throw new DataAccessException($"Failed to retrieve offers for item", ex, _logger);
            }
        }

                /// <summary>
                /// Get offers by vendor ID
                /// </summary>
                public async Task<IEnumerable<VwOffer>> GetOffersByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
                {
                    try
                    {
                        var offers = await _dbContext.Set<VwOffer>()
                            .Where(o => o.VendorId == vendorId)
                            .ToListAsync(cancellationToken);

                return offers;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting offers for vendor ID: {vendorId}");
                throw new DataAccessException($"Failed to retrieve offers for vendor", ex, _logger);
            }
        }

        /// <summary>
        /// Get offers with available stock
        /// </summary>
        public async Task<IEnumerable<TbOffer>> GetAvailableOffersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Getting available offers");

                var offers = await _dbContext.Set<TbOffer>()
                    .AsNoTracking()
                    .Include(o => o.Item)
                    .Include(o => o.Vendor)
                    .Include(o => o.OfferCombinationPricings)
                    .Where(o =>
                        !o.IsDeleted &&
                        o.OfferCombinationPricings.Any(p =>
                            !p.IsDeleted &&
                            p.AvailableQuantity > 0))
                    .ToListAsync(cancellationToken);

                return offers;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting available offers");
                throw new DataAccessException("Failed to retrieve available offers", ex, _logger);
            }
        }

        /// <summary>
        /// Get offer combination pricing by ID
        /// </summary>
        public async Task<TbOfferCombinationPricing> GetOfferCombinationPricingAsync(Guid pricingId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information($"Getting offer pricing for pricing ID: {pricingId}");

                var pricing = await _offerPricing
                    .AsNoTracking()
                    .Include(p => p.Offer)
                    .Include(p => p.ItemCombination)
                    .FirstOrDefaultAsync(p => p.Id == pricingId, cancellationToken);

                return pricing;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting offer pricing for pricing ID: {pricingId}");
                throw new DataAccessException("Failed to retrieve offer pricing", ex, _logger);
            }
        }

        /// <summary>
        /// Get all pricing combinations for an offer
        /// </summary>
        public async Task<IEnumerable<TbOfferCombinationPricing>> GetOfferPricingCombinationsAsync(Guid offerId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information($"Getting pricing combinations for offer ID: {offerId}");

                var pricings = await _offerPricing
                    .AsNoTracking()
                    .Include(p => p.ItemCombination)
                    .Where(p =>
                        p.OfferId == offerId &&
                        !p.IsDeleted)
                    .ToListAsync(cancellationToken);

                return pricings;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting pricing combinations for offer ID: {offerId}");
                throw new DataAccessException("Failed to retrieve offer pricing combinations", ex, _logger);
            }
        }

                /// <summary>
                /// Update offer pricing with transaction support
                /// </summary>
                public async Task<OfferTransactionResult> UpdateOfferPricingAsync(
                     Guid pricingId,
                     decimal? newPrice,
                     decimal? newSalesPrice,
                     string? changeNote,
                     int? availableQty,
                     int? reservedQty,
                     int? refundedQty,
                     int? damagedQty,
                     int? inTransitQty,
                     int? returnedQty,
                     int? lockedQty,
                     string updatedBy,
                     CancellationToken cancellationToken = default)
                {
                    var result = new OfferTransactionResult();
                    var updatedByGuid = Guid.TryParse(updatedBy, out var parsedId) ? parsedId : Guid.Empty;

                    await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                        System.Data.IsolationLevel.ReadCommitted, cancellationToken);

                    try
                    {
                        // Load Pricing
                        var pricing = await _offerPricing
                            .FirstOrDefaultAsync(p => p.Id == pricingId && !p.IsDeleted, cancellationToken);

                        if (pricing == null)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            result.Success = false;
                            return result;
                        }

                        var oldPrice = pricing.Price;
                        var oldSalesPrice = pricing.SalesPrice;
                        var oldAvailableQty = pricing.AvailableQuantity;
                        var oldStockStatus = pricing.StockStatus;

                        changeNote ??= "";
                        bool changesMade = false;
                        bool salesPriceChanged = false;
                        bool stockStatusChanged = false;

                        // ---- Price ----
                        if (newPrice.HasValue && newPrice.Value != pricing.Price)
                        {
                            pricing.Price = newPrice.Value;
                            changesMade = true;
                        }

                        // ---- Sales Price ----
                        if (newSalesPrice.HasValue && newSalesPrice.Value != pricing.SalesPrice)
                        {
                            pricing.SalesPrice = newSalesPrice.Value;
                            changesMade = true;
                            salesPriceChanged = true;
                            changeNote += $"SalesPrice: {oldSalesPrice} → {newSalesPrice.Value}. ";
                        }

                        // ---- Available Quantity + Stock Status Handling ----
                        if (availableQty.HasValue && availableQty.Value != pricing.AvailableQuantity)
                        {
                            pricing.AvailableQuantity = availableQty.Value;
                            changesMade = true;

                            // --- Auto Stock Status Update ---
                            var newStatus =
                                availableQty > pricing.LowStockThreshold ? StockStatus.InStock :
                                availableQty == 0 ? StockStatus.OutOfStock : StockStatus.LimitedStock;
                            if (newStatus != oldStockStatus)
                            {
                                pricing.StockStatus = newStatus;
                                stockStatusChanged = true;
                            }
                        }

                        // ---- Other Quantities ----
                        if (reservedQty.HasValue && reservedQty.Value != pricing.ReservedQuantity)
                        {
                            pricing.ReservedQuantity = reservedQty.Value;
                            changesMade = true;
                        }

                        if (refundedQty.HasValue && refundedQty.Value != pricing.RefundedQuantity)
                        {
                            pricing.RefundedQuantity = refundedQty.Value;
                            changesMade = true;
                        }

                        if (damagedQty.HasValue && damagedQty.Value != pricing.DamagedQuantity)
                        {
                            pricing.DamagedQuantity = damagedQty.Value;
                            changesMade = true;
                        }

                        if (inTransitQty.HasValue && inTransitQty.Value != pricing.InTransitQuantity)
                        {
                            pricing.InTransitQuantity = inTransitQty.Value;
                            changesMade = true;
                        }

                        if (returnedQty.HasValue && returnedQty.Value != pricing.ReturnedQuantity)
                        {
                            pricing.ReturnedQuantity = returnedQty.Value;
                            changesMade = true;
                        }

                        if (lockedQty.HasValue && lockedQty.Value != pricing.LockedQuantity)
                        {
                            pricing.LockedQuantity = lockedQty.Value;
                            changesMade = true;
                        }

                        // No changes → rollback
                        if (!changesMade)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            result.Success = false;
                            return result;
                        }

                        // Metadata
                        pricing.UpdatedDateUtc = DateTime.UtcNow;
                        pricing.UpdatedBy = updatedByGuid;

                _offerPricing.Update(pricing);

                        // ---- Write history ONLY when SalesPrice changed ----
                        if (salesPriceChanged)
                        {
                            var history = new TbOfferPriceHistory
                            {
                                Id = Guid.NewGuid(),
                                OfferCombinationPricingId = pricing.Id,
                                OldPrice = oldSalesPrice,
                                NewPrice = pricing.SalesPrice,
                                ChangeNote = changeNote.Trim(),
                                CreatedBy = updatedByGuid,
                                CreatedDateUtc = DateTime.UtcNow
                            };

                            await _dbContext.TbOfferPriceHistories.AddAsync(history, cancellationToken);
                        }

                        var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                        if (!saveResult)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            result.Success = false;
                            return result;
                        }

                        await transaction.CommitAsync(cancellationToken);

                        result.Success = true;
                        result.PricingId = pricing.Id;
                        result.OfferId = pricing.OfferId;
                        result.NewPrice = pricing.Price;
                        result.NewQuantity = pricing.AvailableQuantity;

                        return result;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        _logger.Error(ex, $"Error updating pricing {pricingId}");
                        throw;
                    }
                }



                /// <summary>
                /// Check if an offer has sufficient stock
                /// </summary>
                public async Task<bool> CheckOfferStockAsync(Guid offerId, Guid itemCombinationId, int quantity, CancellationToken cancellationToken = default)
                {
                    try
                    {
                        var pricing = await _offerPricing
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p =>
                                p.OfferId == offerId &&
                                p.ItemCombinationId == itemCombinationId &&
                                !p.IsDeleted,
                                cancellationToken);

                if (pricing == null)
                {
                    _logger.Warning($"No pricing found for offer {offerId}, combination {itemCombinationId}");
                    return false;
                }

                var availableStock = pricing.AvailableQuantity - pricing.ReservedQuantity;
                var hasStock = availableStock >= quantity;

                return hasStock;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error checking stock for offer {offerId}, combination {itemCombinationId}");
                throw new DataAccessException("Failed to check offer stock", ex, _logger);
            }
        }

        /// <summary>
        /// Reserve stock for an offer
        /// </summary>
        public async Task<OfferTransactionResult> ReserveStockAsync(
            Guid offerId,
            Guid itemCombinationId,
            int quantity,
            string reservedBy,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new OfferTransactionResult();
            var reservedByGuid = Guid.TryParse(reservedBy, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable,
                cancellationToken);

            try
            {
                // Step 1: Get the pricing record with lock
                var pricing = await _offerPricing
                    .FirstOrDefaultAsync(p =>
                        p.OfferId == offerId &&
                        p.ItemCombinationId == itemCombinationId &&
                        !p.IsDeleted,
                        cancellationToken);

                if (pricing == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 2: Check if enough stock is available
                var availableStock = pricing.AvailableQuantity - pricing.ReservedQuantity;
                if (availableStock < quantity)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    transactionResult.ErrorMessage = $"Insufficient stock. Available: {availableStock}, Requested: {quantity}";
                    return transactionResult;
                }

                // Step 3: Update reserved quantity
                pricing.ReservedQuantity += quantity;
                pricing.UpdatedDateUtc = DateTime.UtcNow;
                pricing.UpdatedBy = reservedByGuid;

                _offerPricing.Update(pricing);

                // Step 4: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.OfferId = offerId;
                transactionResult.PricingId = pricing.Id;
                transactionResult.NewQuantity = pricing.AvailableQuantity;
                transactionResult.ReservedQuantity = pricing.ReservedQuantity;

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error reserving stock for offer {offerId}");
                throw new DataAccessException("Failed to reserve stock due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error reserving stock for offer {offerId}");
                throw new DataAccessException("Failed to reserve stock", ex, _logger);
            }
        }

        /// <summary>
        /// Release reserved stock
        /// </summary>
        public async Task<OfferTransactionResult> ReleaseReservedStockAsync(
            Guid offerId,
            Guid itemCombinationId,
            int quantity,
            string releasedBy,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new OfferTransactionResult();
            var releasedByGuid = Guid.TryParse(releasedBy, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable,
                cancellationToken);

            try
            {
                // Step 1: Get the pricing record with lock
                var pricing = await _offerPricing
                    .FirstOrDefaultAsync(p =>
                        p.OfferId == offerId &&
                        p.ItemCombinationId == itemCombinationId &&
                        !p.IsDeleted,
                        cancellationToken);

                if (pricing == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 2: Check if trying to release more than reserved
                if (pricing.ReservedQuantity < quantity)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    transactionResult.ErrorMessage = $"Cannot release more than reserved. Reserved: {pricing.ReservedQuantity}, Requested: {quantity}";
                    return transactionResult;
                }

                // Step 3: Update reserved quantity
                pricing.ReservedQuantity -= quantity;
                pricing.UpdatedDateUtc = DateTime.UtcNow;
                pricing.UpdatedBy = releasedByGuid;

                _offerPricing.Update(pricing);

                // Step 4: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.OfferId = offerId;
                transactionResult.PricingId = pricing.Id;
                transactionResult.NewQuantity = pricing.AvailableQuantity;
                transactionResult.ReservedQuantity = pricing.ReservedQuantity;

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error releasing reserved stock for offer {offerId}");
                throw new DataAccessException("Failed to release reserved stock due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error releasing reserved stock for offer {offerId}");
                throw new DataAccessException("Failed to release reserved stock", ex, _logger);
            }
        }

        /// <summary>
        /// Override base GetAsync to include related data by default
        /// </summary>
        public override async Task<IEnumerable<TbOffer>> GetAsync(
            Expression<Func<TbOffer, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<TbOffer> query = _dbContext.Set<TbOffer>()
                    .AsNoTracking()
                    .Include(o => o.Item)
                    .Include(o => o.Vendor)
                    .Include(o => o.OfferCombinationPricings);

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                query = query.Where(e => !e.IsDeleted);

                        return await query.ToListAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        HandleException(nameof(GetAsync), "Error occurred while filtering active offer entities", ex);
                        return Enumerable.Empty<TbOffer>();
                    }
                }
                public async Task<OfferTransactionResult> CreateOfferAsync(
        TbOffer offer,
        IEnumerable<TbOfferCombinationPricing> pricingList,
        string createdBy,
        CancellationToken cancellationToken = default)
                {
                    var result = new OfferTransactionResult();
                    var createdByGuid = Guid.TryParse(createdBy, out var parsedId) ? parsedId : Guid.Empty;

                    await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                        System.Data.IsolationLevel.Serializable,
                        cancellationToken);

                    try
                    {
                        // Insert Offer
                        offer.Id = Guid.NewGuid();
                        offer.CreatedDateUtc = DateTime.UtcNow;
                        offer.CreatedBy = createdByGuid;

                        await _dbContext.Set<TbOffer>().AddAsync(offer, cancellationToken);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        // Insert Pricing Records
                        foreach (var p in pricingList)
                        {
                            p.Id = Guid.NewGuid();
                            p.OfferId = offer.Id;
                            p.CreatedDateUtc = DateTime.UtcNow;
                            p.CreatedBy = createdByGuid;

                            // Calculate stock status
                            p.StockStatus =
                                p.AvailableQuantity > p.LowStockThreshold ? StockStatus.InStock :
                                p.AvailableQuantity == 0 ? StockStatus.OutOfStock : StockStatus.LimitedStock;

                            await _offerPricing.AddAsync(p, cancellationToken);
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);

                        result.Success = true;
                        result.OfferId = offer.Id;

                        return result;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        _logger.Error(ex, "Error creating new offer");
                        throw new DataAccessException("Failed to create offer", ex, _logger);
                    }
                }
                public async Task<OfferTransactionResult> UpdateOfferAsync(
        TbOffer offer,
        IEnumerable<TbOfferCombinationPricing> pricingList,
        string updatedBy,
        CancellationToken cancellationToken = default)
                {
                    var result = new OfferTransactionResult();
                    var updatedByGuid = Guid.TryParse(updatedBy, out var parsedId) ? parsedId : Guid.Empty;

                    await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                        System.Data.IsolationLevel.Serializable,
                        cancellationToken);

                    try
                    {
                        // Update Offer
                        offer.UpdatedDateUtc = DateTime.UtcNow;
                        offer.UpdatedBy = updatedByGuid;

                        _dbContext.Set<TbOffer>().Update(offer);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        // Update Pricing List
                        foreach (var p in pricingList)
                        {
                            var existing = await _offerPricing
                                .FirstOrDefaultAsync(x => x.Id == p.Id && !x.IsDeleted, cancellationToken);

                            if (existing == null)
                            {
                                // New Pricing
                                p.Id = Guid.NewGuid();
                                p.OfferId = offer.Id;
                                p.CreatedDateUtc = DateTime.UtcNow;
                                p.CreatedBy = updatedByGuid;
                                await _offerPricing.AddAsync(p, cancellationToken);
                            }
                            else
                            {
                                // Price History
                                if (existing.Price != p.Price)
                                {
                                    await _dbContext.AddAsync(new TbOfferPriceHistory
                                    {
                                        Id = Guid.NewGuid(),
                                        OfferCombinationPricingId = existing.Id,
                                        OldPrice = existing.Price,
                                        NewPrice = p.Price,
                                        UpdatedDateUtc = DateTime.UtcNow,
                                        UpdatedBy = updatedByGuid
                                    }, cancellationToken);
                                }

                                // Update Existing
                                existing.Price = p.Price;
                                existing.SalesPrice = p.SalesPrice;
                                existing.AvailableQuantity = p.AvailableQuantity;
                                existing.LowStockThreshold = p.LowStockThreshold;
                                existing.StockStatus =
                                    p.AvailableQuantity > p.LowStockThreshold ? StockStatus.InStock :
                                    p.AvailableQuantity == 0 ? StockStatus.OutOfStock : StockStatus.LimitedStock;

                                existing.UpdatedDateUtc = DateTime.UtcNow;
                                existing.UpdatedBy = updatedByGuid;

                                _offerPricing.Update(existing);
                            }
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);

                        result.Success = true;
                        result.OfferId = offer.Id;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        _logger.Error(ex, "Error updating offer");
                        throw new DataAccessException("Failed to update offer", ex, _logger);
                    }
                }
    }
}