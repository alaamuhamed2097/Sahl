using DAL.ResultModels.DAL.ResultModels;
using Domains.Entities.Offer;

namespace DAL.Contracts.Repositories;

/// <summary>
/// Interface for offer repository operations
/// </summary>
public interface IOfferRepository : ITableRepository<TbOffer>
{
    /// <summary>
    /// Get offer with all related data (item, user, pricing combinations)
    /// </summary>
    Task<TbOffer> GetOfferWithDetailsAsync(Guid offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by item ID
    /// </summary>
    Task<IEnumerable<TbOffer>> GetOffersByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by vendor ID
    /// </summary>
    Task<IEnumerable<TbOffer>> GetOffersByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers with available stock information
    /// </summary>
    Task<IEnumerable<TbOffer>> GetAvailableOffersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offer combination pricing by ID
    /// </summary>
    Task<TbOfferCombinationPricing> GetOfferCombinationPricingAsync(Guid pricingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all pricing combinations for an offer
    /// </summary>
    Task<IEnumerable<TbOfferCombinationPricing>> GetOfferPricingCombinationsAsync(Guid offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update offer pricing with transaction support
    /// </summary>
    Task<OfferTransactionResult> UpdateOfferPricingAsync(
        Guid pricingId,
        decimal newPrice,
        int newQuantity,
        string updatedBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if an offer has sufficient stock for a requested quantity
    /// </summary>
    Task<bool> CheckOfferStockAsync(Guid offerId, Guid itemCombinationId, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reserve stock for an offer (typically during checkout)
    /// </summary>
    Task<OfferTransactionResult> ReserveStockAsync(
        Guid offerId,
        Guid itemCombinationId,
        int quantity,
        string reservedBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Release reserved stock (typically when order is cancelled)
    /// </summary>
    Task<OfferTransactionResult> ReleaseReservedStockAsync(
        Guid offerId,
        Guid itemCombinationId,
        int quantity,
        string releasedBy,
        CancellationToken cancellationToken = default);
}