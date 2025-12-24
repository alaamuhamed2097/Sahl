using Common.Enumerations.Pricing;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Contracts.Service.Catalog.Pricing
{
    /// <summary>
    /// Pricing strategy interface
    /// </summary>
    public interface IPricingStrategy
    {
        PricingResult CalculatePrice(PricingContext context);
        bool CanHandle(PricingStrategyType strategyType);
    }

    public class PricingContext
    {
        public TbItemCombination ItemCombination { get; set; }
        public int RequestedQuantity { get; set; } = 1;
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
        public PricingStrategyType Strategy { get; set; }
    }
}
