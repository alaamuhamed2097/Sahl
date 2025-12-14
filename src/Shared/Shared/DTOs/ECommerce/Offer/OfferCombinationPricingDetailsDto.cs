using Common.Enumerations.Offer;
using Shared.DTOs.Base;
using Shared.DTOs.ECommerce.Item;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferCombinationPricingDetailsDto : BaseDto
    {
        public Guid ItemCombinationId { get; set; }
        public string? Barcode { get; set; }
        public string? SKU { get; set; }
        public bool IsDefault { get; set; }
        public Guid OfferId { get; set; }

        public Guid OfferConditionId { get; set; }
        public string ConditionNameAr {  get; set; }
        public string ConditionNameEn {  get; set; }
        public bool IsConditionNew {  get; set; }
        public decimal Price { get; set; }

        public decimal SalesPrice { get; set; }

        public decimal? CostPrice { get; set; }

        public int AvailableQuantity { get; set; }

        public int ReservedQuantity { get; set; }

        public int RefundedQuantity { get; set; }

        public int DamagedQuantity { get; set; }

        public int InTransitQuantity { get; set; }

        public int ReturnedQuantity { get; set; }

        public int LockedQuantity { get; set; }

        public StockStatus StockStatus { get; set; }
        public DateTime? LastStockUpdate { get; set; }

        // Stock Management
        public int MinOrderQuantity { get; set; } = 1;
        public int MaxOrderQuantity { get; set; } = 999;
        public int LowStockThreshold { get; set; } = 5;

        // Navigation properties
        public virtual List<ItemCombinationImageDto> ItemCombinationImages { get; set; } = new List<ItemCombinationImageDto>();
        public virtual List<CombinationAttributeDto> CombinationAttributes  { get; set; } = new List<CombinationAttributeDto>();
    }

}