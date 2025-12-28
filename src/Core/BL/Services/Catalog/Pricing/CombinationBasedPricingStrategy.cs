using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

// ==================== Combination-Based Pricing Strategy ====================

/// <summary>
/// Combination-based pricing (attribute/variant based)
/// Each combination has its own pricing
/// </summary>
public class CombinationBasedPricingStrategy : IPricingStrategy
{
    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.CombinationBased;
    }

    public PricingResult CalculatePrice(PricingContext context)
    {
        var combination = context.ItemCombination;
        var now = context.CalculationDate;

        // Get best offer for this specific combination
        var bestOffer = combination.OfferCombinationPricings
            ?.Where(op => !op.IsDeleted)
            .OrderBy(op => op.SalesPrice)
            .FirstOrDefault();

        if (bestOffer == null)
        {
            return new PricingResult
            {
                Price = combination.Item.BasePrice ?? 0,
                SalesPrice = combination.Item.BasePrice ?? 0,
                IsAvailable = false
            };
        }

        return new PricingResult
        {
            Price = bestOffer.Price,
            SalesPrice = bestOffer.SalesPrice,
            IsAvailable = bestOffer.AvailableQuantity >= context.RequestedQuantity,
            ActiveOfferId = bestOffer.OfferId
        };
    }
}
