using Domains.Entities.Catalog.Item.ItemAttributes;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Contracts.Service.Catalog.Pricing;

/// <summary>
/// Main pricing service that uses the appropriate strategy
/// </summary>
public interface IPricingService
{
    PricingResult CalculatePrice(PricingContext context);
    PricingResult GetBestOffer(TbItemCombination combination, int quantity = 1);
}
