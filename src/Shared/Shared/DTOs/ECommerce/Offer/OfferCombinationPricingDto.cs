using Common.Enumerations.Offer;
using Shared.DTOs.Base;
using Shared.DTOs.Catalog.Item;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferCombinationPricingDto : BaseDto
    {
        public Guid ItemCombinationId { get; set; }

        public Guid OfferId { get; set; }

        public Guid OfferConditionId { get; set; }

        public string Barcode { get; set; } = "Default";

        public string SKU { get; set; } = "Default";

        public decimal Price { get; set; }

        public decimal SalesPrice { get; set; }

        public int AvailableQuantity { get; set; }

        // Stock Management
        public int MinOrderQuantity { get; set; } = 1;
        public int MaxOrderQuantity { get; set; } = 999;
        public int LowStockThreshold { get; set; } = 5;

        // Navigation properties
        public virtual List<ItemCombinationDto>? ItemCombinationDtos  { get; set; } 
    }

}