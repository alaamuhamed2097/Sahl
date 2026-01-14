using DAL.ResultModels.DAL.ResultModels;
using Domains.Entities.Offer;
using Domains.Views.Offer;

namespace DAL.Contracts.Repositories;

/// <summary>
/// Interface for vendor item repository operations
/// </summary>
public interface IVendorItemRepository : ITableRepository<TbOffer>
{
    /// <summary>
    /// Get vendor item with all related data (item, user, pricing combinations)
    /// </summary>
    Task<VwOffer> GetOfferWithDetailsAsync(Guid offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by item combination ID
    /// </summary>
    Task<IEnumerable<VwVendorItem>> GetOffersByItemCombinationIdAsync(Guid itemCombinationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers by vendor ID
    /// </summary>
    Task<IEnumerable<VwOffer>> GetOffersByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get offers with available stock information
    /// </summary>
    Task<IEnumerable<TbOffer>> GetAvailableOffersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get vendor item combination pricing by ID
    /// </summary>
    Task<TbOfferCombinationPricing> GetOfferCombinationPricingAsync(Guid pricingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all pricing combinations for an offer
    /// </summary>
    Task<IEnumerable<TbOfferCombinationPricing>> GetOfferPricingCombinationsAsync(Guid offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update vendor item pricing and stock quantities
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
    /// Check if an vendor item has sufficient stock for a requested quantity
    /// </summary>
    Task<bool> CheckOfferStockAsync(Guid offerId, Guid itemCombinationId, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reserve stock for an vendor item (typically during checkout)
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
    /// Create a new vendor item with its pricing combinations
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
    /// Update an existing vendor item and its pricing combinations
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


    /// <summary>
    /// Asynchronously retrieves a collection of offers that are associated with the specified combination pricing
    /// identifiers.
    /// </summary>
    /// <param name="pricingIds">A collection of combination pricing identifiers used to filter the offers. Cannot be null or contain duplicate
    /// values.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of offers
    /// matching the specified combination pricing identifiers. The collection is empty if no matching offers are found.</returns>
    Task<IEnumerable<TbOffer>> GetOffersByCombinationPricingIdsAsync(IEnumerable<Guid> pricingIds, CancellationToken cancellationToken = default);
}