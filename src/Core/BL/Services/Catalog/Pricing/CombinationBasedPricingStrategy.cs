using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using DAL.Contracts.Repositories;
using Domains.Entities.Offer;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

// ==================== Combination-Based Pricing Strategy ====================

/// <summary>
/// Combination-based pricing (attribute/variant based)
/// Each combination has its own pricing
/// </summary>
public class CombinationBasedPricingStrategy : IPricingStrategy
{
    private readonly ITableRepository<TbOfferCombinationPricing> _combinationPricingRepository;
    public CombinationBasedPricingStrategy(ITableRepository<TbOfferCombinationPricing> combinationPricingRepository)
    {
        _combinationPricingRepository = combinationPricingRepository;
    }

    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.CombinationBased;
    }

    public async Task<PricingResult> CalculatePrice(
        Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity)
    {
        // Get pricing for the specific combination
        var pricing = await _combinationPricingRepository
            .FindAsync(op => !op.IsDeleted && op.ItemCombinationId == itemCombinationId);

        if (pricing == null)
            throw new KeyNotFoundException("Combination pricing not found for itemCombinationId " + itemCombinationId);

        return new PricingResult
        {
            Price = pricing.Price,
            SalesPrice = pricing.SalesPrice,
            IsAvailable = pricing.AvailableQuantity >= requestedQuantity,
            ActiveOfferId = pricing.OfferId
        };
    }
}
