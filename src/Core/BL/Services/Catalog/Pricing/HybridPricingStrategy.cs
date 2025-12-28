using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

// ==================== Hybrid Pricing Strategy ====================

/// <summary>
/// Hybrid: combination + quantity tiers
/// Different combinations with quantity-based pricing
/// </summary>
public class HybridPricingStrategy : IPricingStrategy
{
    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.Hybrid;
    }

    public PricingResult CalculatePrice(PricingContext context)
    {
        var combination = context.ItemCombination;
        var requestedQty = context.RequestedQuantity;
        var now = context.CalculationDate;

        // Get all active offers for this combination
        var activeOffers = combination.OfferCombinationPricings
            .Where(op => !op.IsDeleted)
            .OrderByDescending(op => op.SalesPrice)
            .ToList();

        if (activeOffers == null || !activeOffers.Any())
            throw new InvalidOperationException("No active offer found for the item combination.");

        // Find best tier for quantity
        var applicableOffer = activeOffers
            .FirstOrDefault(op => requestedQty >= op.MinOrderQuantity);

        if (applicableOffer == null)
        {
            applicableOffer = activeOffers.OrderBy(op => op.MinOrderQuantity).First();
        }

        var totalAvailable = activeOffers.Sum(op => op.AvailableQuantity);

        return new PricingResult
        {
            Price = applicableOffer.Price,
            SalesPrice = applicableOffer.SalesPrice,
            IsAvailable = totalAvailable >= requestedQty,
            ActiveOfferId = applicableOffer.OfferId,
        };
    }
}
