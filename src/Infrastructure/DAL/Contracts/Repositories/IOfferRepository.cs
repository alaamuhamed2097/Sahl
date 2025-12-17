using DAL.ResultModels.DAL.ResultModels;
using Domains.Entities.Offer;
using Domains.Views.Offer;

namespace DAL.Contracts.Repositories;

/// <summary>
/// Interface for offer repository operations
/// </summary>
public interface IOfferRepository : ITableRepository<TbOffer>
{
    /// <summary>
    /// Get offer with all related data (item, user, pricing combinations)
    /// </summary>
    Task<VwOffer> GetOfferWithDetailsAsync(Guid offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by item ID
    /// </summary>
    Task<IEnumerable<VwOffer>> GetOffersByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by vendor ID
    /// </summary>
    Task<IEnumerable<VwOffer>> GetOffersByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);

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
    /// Update offer pricing and stock quantities
    /// </summary>
    /// <param name="pricingId"></param>
    /// <param name="newPrice"></param>
    /// <param name="newSalesPrice"></param>
    /// <param name="changeNote"></param>
    /// <param name="availableQty"></param>
    /// <param name="reservedQty"></param>
    /// <param name="refundedQty"></param>
    /// <param name="damagedQty"></param>
    /// <param name="inTransitQty"></param>
    /// <param name="returnedQty"></param>
    /// <param name="lockedQty"></param>
    /// <param name="updatedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OfferTransactionResult> UpdateOfferPricingAsync(
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

    /// <summary>
    /// Create a new offer with its pricing combinations
    /// </summary>
    /// <param name="offer"></param>
    /// <param name="pricingList"></param>
    /// <param name="createdBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OfferTransactionResult> CreateOfferAsync(
    TbOffer offer,
    IEnumerable<TbOfferCombinationPricing> pricingList,
    string createdBy,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing offer and its pricing combinations
    /// </summary>
    /// <param name="offer"></param>
    /// <param name="pricingList"></param>
    /// <param name="updatedBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OfferTransactionResult> UpdateOfferAsync(
    TbOffer offer,
    IEnumerable<TbOfferCombinationPricing> pricingList,
    string updatedBy,
    CancellationToken cancellationToken = default);
}