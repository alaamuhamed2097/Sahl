using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Service.Catalog.Pricing
{
    public class SimplePricingStrategy : IPricingStrategy
    {
        public bool CanHandle(PricingStrategyType strategyType)
        {
            return strategyType == PricingStrategyType.Simple;
        }

        public PricingResult CalculatePrice(PricingContext context)
        {
            var combination = context.ItemCombination;

            var activeOffer = combination.OfferCombinationPricings
                ?.FirstOrDefault(op =>
                    !op.IsDeleted);

            if (activeOffer == null)
                throw new InvalidOperationException("No active offer found for the item combination.");

            return new PricingResult
            {
                Price = activeOffer.Price,
                SalesPrice = activeOffer.SalesPrice,
                IsAvailable = activeOffer.AvailableQuantity > 0,
                ActiveOfferId = activeOffer.OfferId
            };
        }
    }
}