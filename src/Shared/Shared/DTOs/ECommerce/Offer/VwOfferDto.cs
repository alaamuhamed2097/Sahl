using Shared.DTOs.ECommerce.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.ECommerce.Offer
{
    public class VwOfferDto
    {
            // Offer basic fields
            public Guid Id { get; set; }
            public Guid ItemId { get; set; }
            public Guid VendorId { get; set; }
            public int StorgeLocation { get; set; }
            public int HandlingTimeInDays { get; set; }
            public int VisibilityScope { get; set; }
            public int FulfillmentType { get; set; }
            public decimal VendorRatingForThisItem { get; set; }
            public int VendorSalesCountForThisItem { get; set; }
            public bool IsBuyBoxWinner { get; set; }
            public Guid? WarrantyId { get; set; }
            public DateTime CreatedDateUtc { get; set; }

            // Vendor info
            public string VendorFullName { get; set; } = string.Empty;
            public string VendorCompanyName { get; set; } = string.Empty;
            public string VendorEmail { get; set; } = string.Empty;
            public string VendorFullPhoneNumber { get; set; } = string.Empty;

            // Item fields
            public string ItemTitleAr { get; set; } = string.Empty;
            public string ItemTitleEn { get; set; } = string.Empty;
            public Guid BrandId { get; set; }
            public string BrandTitleAr { get; set; } = string.Empty;
            public string BrandTitleEn { get; set; } = string.Empty;
            public Guid CategoryId { get; set; }
            public string CategoryTitleAr { get; set; } = string.Empty;
            public string CategoryTitleEn { get; set; } = string.Empty;
            public int PricingSystemType { get; set; }

            // Collections
            public List<ItemImageDto> ItemImages { get; set; } = new List<ItemImageDto>();
            public List<ItemAttributeDto> ItemAttributes { get; set; } = new List<ItemAttributeDto>();
            public List<OfferCombinationPricingDto> OfferCombinations { get; set; } = new List<OfferCombinationPricingDto>();

            // Warranty
            public int? WarrantyType { get; set; }
            public int? WarrantyPeriodMonths { get; set; }
            public string? WarrantyPolicy { get; set; }
    }
}
