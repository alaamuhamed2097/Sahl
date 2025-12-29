using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

public class PricingService : IPricingService
{
    private readonly Dictionary<PricingStrategyType, IPricingStrategy> _strategies;

    public PricingService(
        SimplePricingStrategy simplePricing,
        CombinationBasedPricingStrategy combinationPricing,
        QuantityBasedPricingStrategy quantityPricing,
        HybridPricingStrategy hybridPricing)
    {
        _strategies = new Dictionary<PricingStrategyType, IPricingStrategy>
        {
            { PricingStrategyType.Simple, simplePricing },
            { PricingStrategyType.CombinationBased, combinationPricing },
            { PricingStrategyType.QuantityBased, quantityPricing },
            { PricingStrategyType.Hybrid, hybridPricing }
        };
    }

    public async Task<PricingResult> CalculatePrice(Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity)
    {
        if (itemCombinationId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(itemCombinationId));
        }

        if (!_strategies.TryGetValue(strategyType, out var strategy))
        {
            throw new InvalidOperationException(
                $"No pricing strategy found for type {strategyType}");
        }

        return await strategy.CalculatePrice(itemCombinationId, strategyType, requestedQuantity);
    }
}