using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using DAL.Contracts.Repositories;
using Domains.Entities.Offer;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

// ==================== Hybrid Pricing Strategy ====================

/// <summary>
/// Hybrid: combination + quantity tiers
/// Different combinations with quantity-based pricing
/// </summary>
public class HybridPricingStrategy : IPricingStrategy
{
    private readonly ITableRepository<TbOfferCombinationPricing> _combinationPricingRepository;
    public HybridPricingStrategy(ITableRepository<TbOfferCombinationPricing> combinationPricingRepository)
    {
        _combinationPricingRepository = combinationPricingRepository;
    }

    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.Hybrid;
    }

    public Task<PricingResult> CalculatePrice(
        Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity)
    {
        throw new NotImplementedException();
    }
}
