using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Service.Catalog.Pricing
{
    // ==================== Quantity-Based Pricing Strategy ====================

    /// <summary>
    /// Quantity-based (tiered) pricing
    /// Price changes based on quantity purchased
    /// </summary>
    public class QuantityBasedPricingStrategy : IPricingStrategy
    {
        public bool CanHandle(PricingStrategyType strategyType)
        {
            return strategyType == PricingStrategyType.QuantityBased;
        }

        public PricingResult CalculatePrice(PricingContext context)
        {
            var combination = context.ItemCombination;
            var requestedQty = context.RequestedQuantity;
            var now = context.CalculationDate;

            // Get all active offers sorted by minimum quantity
            var activeOffers = combination.OfferCombinationPricings
                ?.Where(op => !op.IsDeleted)
                .OrderByDescending(op => op.MinOrderQuantity)
                .ToList();

            if (activeOffers == null || !activeOffers.Any())
                throw new InvalidOperationException("No active offer found for the item combination.");

            // Find the best tier for requested quantity
            var applicableOffer = activeOffers
                .FirstOrDefault(op => requestedQty >= op.MinOrderQuantity);

            if (applicableOffer == null)
            {
                // Use the lowest tier
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

}
