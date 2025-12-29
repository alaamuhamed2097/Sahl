using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using DAL.Contracts.Repositories;
using Domains.Entities.Offer;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

// ==================== Quantity-Based Pricing Strategy ====================

/// <summary>
/// Quantity-based (tiered) pricing
/// Price changes based on quantity purchased
/// </summary>
public class QuantityBasedPricingStrategy : IPricingStrategy
{
    private readonly ITableRepository<TbOfferCombinationPricing> _combinationPricingRepository;
    public QuantityBasedPricingStrategy(ITableRepository<TbOfferCombinationPricing> combinationPricingRepository)
    {
        _combinationPricingRepository = combinationPricingRepository;
    }

    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.QuantityBased;
    }

    public Task<PricingResult> CalculatePrice(
        Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity)
    {
        throw new NotImplementedException();
    }
}
