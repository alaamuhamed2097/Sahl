using BL.Contracts.Service.ECommerce.Item;
using DAL.Contracts.Repositories;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Shared.DTOs.ECommerce.Item;
using System.Text.Json;

namespace BL.Service.ECommerce.Item
{
    public class ItemDetailsService : IItemDetailsService
    {
        private readonly IItemDetailsRepository _repository;

        public ItemDetailsService(IItemDetailsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemId)
        {
            var result = await _repository.GetItemDetailsAsync(itemId);

            if (result == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found");
            }

            return MapToDto(result);
        }

        public async Task<CombinationDetailsDto> GetCombinationByAttributesAsync(
            Guid itemId,
            GetCombinationRequest request)
        {
            if (request?.SelectedAttributes == null || !request.SelectedAttributes.Any())
            {
                throw new ArgumentException("Selected attributes are required");
            }

            var selections = request.SelectedAttributes
                .Select(a => new AttributeSelection
                {
                    AttributeId = a.AttributeId,
                    ValueId = a.ValueId
                })
                .ToList();

            var result = await _repository.GetCombinationByAttributesAsync(itemId, selections);

            if (result == null)
            {
                throw new KeyNotFoundException($"Could not process combination for item {itemId}");
            }

            return MapCombinationToDto(result);
        }

        #region Private Mapping Methods

        private ItemDetailsDto MapToDto(SpGetItemDetails result)
        {
            var dto = new ItemDetailsDto
            {
                Id = result.Id,
                TitleAr = result.TitleAr,
                TitleEn = result.TitleEn,
                DescriptionAr = result.DescriptionAr,
                DescriptionEn = result.DescriptionEn,
                ShortDescriptionAr = result.ShortDescriptionAr,
                ShortDescriptionEn = result.ShortDescriptionEn,
                ThumbnailImage = result.ThumbnailImage,
                HasCombinations = result.HasCombinations,
                IsMultiVendor = result.IsMultiVendor,
                PricingSystemType = result.PricingSystemType,
                PricingSystemName = result.PricingSystemNameEn, // or use NameAr based on culture
                AverageRating = result.AverageRating,

                Category = new CategoryInfoDto
                {
                    Id = result.CategoryId,
                    NameAr = result.CategoryNameAr,
                    NameEn = result.CategoryNameEn
                },

                Brand = result.BrandId.HasValue ? new BrandInfoDto
                {
                    Id = result.BrandId,
                    NameAr = result.BrandNameAr,
                    NameEn = result.BrandNameEn,
                    LogoUrl = result.BrandLogoUrl
                } : null
            };

            // Parse JSON fields
            dto.GeneralImages = ParseGeneralImages(result.GeneralImagesJson);
            dto.Attributes = ParseAttributes(result.AttributesJson);
            dto.DefaultCombination = ParseDefaultCombination(result.DefaultCombinationJson);
            dto.Pricing = ParsePricing(result.PricingJson);

            return dto;
        }

        private List<ItemImageDto> ParseGeneralImages(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<ItemImageDto>();

            try
            {
                var images = JsonSerializer.Deserialize<List<ItemImage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return images?.Select(img => new ItemImageDto
                {
                    Path = img.ImageUrl,
                    Order = img.DisplayOrder
                }).ToList() ?? new List<ItemImageDto>();
            }
            catch
            {
                return new List<ItemImageDto>();
            }
        }

        private List<ItemAttributeDefinitionDto> ParseAttributes(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<ItemAttributeDefinitionDto>();

            try
            {
                var attributes = JsonSerializer.Deserialize<List<AttributeInfo>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return attributes?.Select(attr => new ItemAttributeDefinitionDto
                {
                    AttributeId = attr.AttributeId,
                    NameAr = attr.NameAr,
                    NameEn = attr.NameEn,
                    FieldType = attr.FieldType,
                    DisplayOrder = attr.DisplayOrder,
                    AffectsPricing = attr.AffectsPricing,
                    IsRequired = attr.IsRequired,

                    // Pricing attributes have options
                    Options = attr.AffectsPricing && attr.Values != null
                        ? attr.Values.Select(v => new ItemAttributeOptionDto
                        {
                            ValueId = v.ValueId,
                            ValueAr = v.ValueAr,
                            ValueEn = v.ValueEn,
                            DisplayOrder = v.DisplayOrder,
                            IsAvailable = v.IsAvailable
                        }).ToList()
                        : null,

                    // Spec attributes have single value
                    Value = !attr.AffectsPricing && attr.Value != null
                        ? new AttributeValueDto
                        {
                            ValueAr = attr.Value.ValueAr,
                            ValueEn = attr.Value.ValueEn
                        }
                        : null
                }).ToList() ?? new List<ItemAttributeDefinitionDto>();
            }
            catch
            {
                return new List<ItemAttributeDefinitionDto>();
            }
        }

        private DefaultCombinationDto ParseDefaultCombination(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var combo = JsonSerializer.Deserialize<DefaultCombination>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (combo == null)
                    return null;

                return new DefaultCombinationDto
                {
                    CombinationId = combo.CombinationId,
                    SKU = combo.SKU,
                    SelectedAttributes = combo.SelectedAttributes?.Select(sa => new SelectedAttributeDto
                    {
                        AttributeId = sa.AttributeId,
                        AttributeNameAr = sa.AttributeNameAr,
                        AttributeNameEn = sa.AttributeNameEn,
                        ValueId = sa.ValueId,
                        ValueAr = sa.ValueAr,
                        ValueEn = sa.ValueEn
                    }).ToList(),
                    Images = combo.Images?.Select(img => new ItemImageDto
                    {
                        Path = img.ImageUrl,
                        Order = img.DisplayOrder
                    }).ToList()
                };
            }
            catch
            {
                return null;
            }
        }

        private PricingDto ParsePricing(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var pricing = JsonSerializer.Deserialize<PricingInfo>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (pricing == null)
                    return null;

                return new PricingDto
                {
                    VendorCount = pricing.VendorCount,
                    MinPrice = pricing.MinPrice,
                    MaxPrice = pricing.MaxPrice,
                    BestOffer = pricing.BestOffer != null ? new BestPriceOfferDto
                    {
                        OfferId = pricing.BestOffer.OfferId,
                        VendorId = pricing.BestOffer.VendorId,
                        VendorName = pricing.BestOffer.VendorName,
                        VendorRating = pricing.BestOffer.VendorRating,
                        Price = pricing.BestOffer.Price,
                        SalesPrice = pricing.BestOffer.SalesPrice,
                        DiscountPercentage = pricing.BestOffer.DiscountPercentage,
                        AvailableQuantity = pricing.BestOffer.AvailableQuantity,
                        StockStatus = pricing.BestOffer.StockStatus,
                        IsFreeShipping = pricing.BestOffer.IsFreeShipping,
                        EstimatedDeliveryDays = pricing.BestOffer.EstimatedDeliveryDays,
                        IsBuyBoxWinner = pricing.BestOffer.IsBuyBoxWinner,
                        MinOrderQuantity = pricing.BestOffer.MinOrderQuantity,
                        MaxOrderQuantity = pricing.BestOffer.MaxOrderQuantity,
                        QuantityTiers = pricing.BestOffer.QuantityTiers?.Select(qt => new QuantityTierDto
                        {
                            MinQuantity = qt.MinQuantity,
                            MaxQuantity = qt.MaxQuantity,
                            UnitPrice = qt.UnitPrice
                        }).ToList()
                    } : null
                };
            }
            catch
            {
                return null;
            }
        }

        private CombinationDetailsDto MapCombinationToDto(SpGetAvailableOptionsForSelection result)
        {
            var dto = new CombinationDetailsDto
            {
                CombinationId = result.CombinationId,
                SKU = result.SKU,
                Barcode = result.Barcode,
                IsAvailable = result.IsAvailable,
                Message = result.Message
            };

            // Parse JSON fields
            dto.SelectedAttributes = ParseSelectedAttributes(result.SelectedAttributesJson);
            dto.Images = ParseImages(result.ImagesJson);
            dto.Offers = ParseOffers(result.OffersJson);
            dto.Summary = ParseSummary(result.SummaryJson);
            dto.MissingAttributes = ParseMissingAttributes(result.MissingAttributesJson);

            return dto;
        }

        private List<SelectedAttributeDto> ParseSelectedAttributes(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<SelectedAttributeDto>();

            try
            {
                var attrs = JsonSerializer.Deserialize<List<SelectedAttribute>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return attrs?.Select(a => new SelectedAttributeDto
                {
                    AttributeId = a.AttributeId,
                    AttributeNameAr = a.AttributeNameAr,
                    AttributeNameEn = a.AttributeNameEn,
                    ValueId = a.ValueId,
                    ValueAr = a.ValueAr,
                    ValueEn = a.ValueEn
                }).ToList() ?? new List<SelectedAttributeDto>();
            }
            catch
            {
                return new List<SelectedAttributeDto>();
            }
        }

        private List<ItemImageDto> ParseImages(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<ItemImageDto>();

            try
            {
                var images = JsonSerializer.Deserialize<List<ItemImage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return images?.Select(img => new ItemImageDto
                {
                    Path = img.ImageUrl,
                    Order = img.DisplayOrder
                }).ToList() ?? new List<ItemImageDto>();
            }
            catch
            {
                return new List<ItemImageDto>();
            }
        }

        private List<VendorOfferDto> ParseOffers(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<VendorOfferDto>();

            try
            {
                var offers = JsonSerializer.Deserialize<List<VendorOffer>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return offers?.Select(o => new VendorOfferDto
                {
                    OfferId = o.OfferId,
                    VendorId = o.VendorId,
                    VendorName = o.VendorName,
                    VendorNameAr = o.VendorNameAr,
                    VendorRating = o.VendorRating,
                    VendorLogoUrl = o.VendorLogoUrl,
                    Price = o.Price,
                    SalesPrice = o.SalesPrice,
                    DiscountPercentage = o.DiscountPercentage,
                    AvailableQuantity = o.AvailableQuantity,
                    StockStatus = o.StockStatus,
                    IsFreeShipping = o.IsFreeShipping,
                    ShippingCost = o.ShippingCost,
                    EstimatedDeliveryDays = o.EstimatedDeliveryDays,
                    IsBuyBoxWinner = o.IsBuyBoxWinner,
                    HasWarranty = o.HasWarranty,
                    ConditionNameAr = o.ConditionNameAr,
                    ConditionNameEn = o.ConditionNameEn,
                    WarrantyTypeAr = o.WarrantyTypeAr,
                    WarrantyTypeEn = o.WarrantyTypeEn,
                    WarrantyPeriodMonths = o.WarrantyPeriodMonths,
                    MinOrderQuantity = o.MinOrderQuantity,
                    MaxOrderQuantity = o.MaxOrderQuantity,
                    OfferRank = o.OfferRank,
                    QuantityTiers = o.QuantityTiers?.Select(qt => new QuantityTierDto
                    {
                        MinQuantity = qt.MinQuantity,
                        MaxQuantity = qt.MaxQuantity,
                        UnitPrice = qt.UnitPrice
                    }).ToList()
                }).ToList() ?? new List<VendorOfferDto>();
            }
            catch
            {
                return new List<VendorOfferDto>();
            }
        }

        private CombinationSummaryDto ParseSummary(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var summary = JsonSerializer.Deserialize<CombinationSummary>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (summary == null)
                    return null;

                return new CombinationSummaryDto
                {
                    TotalVendors = summary.TotalVendors,
                    IsMultiVendor = summary.IsMultiVendor,
                    MinPrice = summary.MinPrice,
                    MaxPrice = summary.MaxPrice,
                    AvgPrice = summary.AvgPrice,
                    TotalStock = summary.TotalStock
                };
            }
            catch
            {
                return null;
            }
        }

        private List<MissingAttributeDto> ParseMissingAttributes(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<MissingAttributeDto>();

            try
            {
                var missing = JsonSerializer.Deserialize<List<MissingAttribute>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return missing?.Select(m => new MissingAttributeDto
                {
                    AttributeId = m.AttributeId,
                    NameAr = m.NameAr,
                    NameEn = m.NameEn,
                    Status = m.Status
                }).ToList() ?? new List<MissingAttributeDto>();
            }
            catch
            {
                return new List<MissingAttributeDto>();
            }
        }

        #endregion
    }
}