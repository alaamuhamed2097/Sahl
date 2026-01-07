using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Procedures;
using Domains.Views.Item;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper;

// Item mappings partial (MappingProfile.Items.cs)
public partial class MappingProfile
{
    private void ConfigureItemMappings()
    {
        // Core item mappings
        CreateMap<TbItem, ItemDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ItemImages))
            .ReverseMap();

        CreateMap<TbItem, Item>()
            .ReverseMap();

        CreateMap<TbItemImage, ItemImageViewDto>()
            .ReverseMap();

        CreateMap<VwItem, ItemDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Images)
                    ? new List<ItemImageDto>()
                    : DeserializeItemImages(src.Images)))
            .ForMember(dest => dest.ItemAttributes, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.ItemAttributes)
                    ? JsonSerializer.Deserialize<List<ItemAttributeDto>>(src.ItemAttributes,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<ItemAttributeDto>()
                    : new List<ItemAttributeDto>()))
            .ReverseMap();

        CreateMap<SpGetItemDetails, ItemDetailsDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new CategoryInfoDto
            {
                Id = src.CategoryId,
                NameAr = src.CategoryNameAr,
                NameEn = src.CategoryNameEn
            }))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src =>
                src.BrandId.HasValue
                    ? new BrandInfoDto
                    {
                        Id = src.BrandId,
                        NameAr = src.BrandNameAr,
                        NameEn = src.BrandNameEn,
                        LogoUrl = src.BrandLogoUrl
                    }
                    : new BrandInfoDto()))
            .ForMember(dest => dest.GeneralImages, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.GeneralImagesJson)
                    ? new List<ImageDto>()
                    : DeserializeGeneralImages(src.GeneralImagesJson)))
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.AttributesJson)
                    ? new List<ItemAttributeDefinitionDto>()
                    : DeserializeAttributes(src.AttributesJson)))
            .ForMember(dest => dest.CurrentCombination, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.CurrentCombinationJson)
                    ? null
                    : DeserializeCurrentCombination(src.CurrentCombinationJson)))
            .ForMember(dest => dest.Pricing, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.PricingJson)
                    ? null
                    : DeserializePricing(src.PricingJson)))
            .ReverseMap();


        // Search result mappings
        CreateMap<SpSearchItemsMultiVendor, SearchItemDto>()
            .ReverseMap();

        // Item combination mapping
        CreateMap<TbItemCombination, ItemCombinationDto>()
            .ReverseMap();

        CreateMap<TbItemCombinationImage, ItemCombinationDto>()
            .ReverseMap();

        // Item attribute mappings
        CreateMap<TbItemAttribute, ItemAttributeDto>()
            .ReverseMap();

        // Combination attribute value mappings
        CreateMap<TbCombinationAttributesValue, CombinationAttributeValueDto>()
            .ReverseMap();

        CreateMap<TbCombinationAttribute, CombinationAttributeDto>()
            .ReverseMap();

        // Item images
        CreateMap<TbItemImage, ItemImageDto>()
            .ReverseMap();
    }

    private static List<ItemImageDto> DeserializeItemImages(string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null, // Don't convert property names
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<List<ItemImageDto>>(json, options) ?? new List<ItemImageDto>();
        }
        catch (JsonException)
        {
            // You might want to log this exception
            // For now, return empty list
            return new List<ItemImageDto>();
        }
    }

    private static List<ItemCombinationDto> DeserializeItemCombinations(string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null, // Don't convert property names
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<List<ItemCombinationDto>>(json, options) ?? new List<ItemCombinationDto>();
        }
        catch (JsonException)
        {
            // You might want to log this exception
            // For now, return empty list
            return new List<ItemCombinationDto>();
        }
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static List<ImageDto> DeserializeGeneralImages(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new();

        try
        {
            var images = JsonSerializer.Deserialize<List<ItemImage>>(json, JsonOptions);
            return images?.Select(img => new ImageDto
            {
                Id = img.Id,
                Path = img.ImageUrl,
                Order = img.DisplayOrder,
                IsDefault = img.IsDefault,
            }).ToList() ?? new();
        }
        catch
        {
            return new();
        }
    }

    private static List<ItemAttributeDefinitionDto> DeserializeAttributes(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new();

        try
        {
            var attributes = JsonSerializer.Deserialize<List<AttributeInfo>>(json, JsonOptions);

            return attributes?.Select(attr => new ItemAttributeDefinitionDto
            {
                AttributeId = attr.AttributeId,
                NameAr = attr.NameAr,
                NameEn = attr.NameEn,
                FieldType = attr.FieldType,
                DisplayOrder = attr.DisplayOrder,
                ValueAr = attr.ValueAr,
                ValueEn = attr.ValueEn
            }).ToList() ?? new List<ItemAttributeDefinitionDto>();
        }
        catch
        {
            return new();
        }
    }

    private static CurrentCombinationDto DeserializeCurrentCombination(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            var combo = JsonSerializer.Deserialize<CurrentCombination>(json, JsonOptions);
            if (combo == null) return null;

            return new CurrentCombinationDto
            {
                CombinationId = combo.CombinationId,
                SKU = combo.SKU,
                Barcode = combo.Barcode,
                IsDefault = combo.IsDefault,
                CreatedBy = combo.CreatedBy,
                PricingAttributes = combo.PricingAttributes?.Select(sa => new PricingAttributeDto
                {
                    AttributeId = sa.AttributeId,
                    AttributeNameAr = sa.AttributeNameAr,
                    AttributeNameEn = sa.AttributeNameEn,
                    CombinationValueId = sa.CombinationValueId,
                    ValueAr = sa.ValueAr,
                    ValueEn = sa.ValueEn,
                    IsSelected = sa.IsSelected
                }).ToList(),
                Images = combo.Images?.Select(img => new ImageDto
                {
                    Id = img.Id,
                    Path = img.ImageUrl,
                    Order = img.DisplayOrder,
                    IsDefault = img.IsDefault,
                }).ToList()
            };
        }
        catch
        {
            return null;
        }
    }

    // ✅ FIXED: Handle both string and object types for BestOffer (backward compatible)
    private static PricingDto DeserializePricing(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            // First, deserialize to a dynamic structure to check BestOfferJson type
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var vendorCount = root.GetProperty("VendorCount").GetInt32();
            var minPrice = root.GetProperty("MinPrice").GetDecimal();
            var maxPrice = root.GetProperty("MaxPrice").GetDecimal();

            BestOffer? bestOffer = null;

            if (root.TryGetProperty("BestOfferJson", out var bestOfferElement))
            {
                // Check if BestOfferJson is a string or object
                if (bestOfferElement.ValueKind == JsonValueKind.String)
                {
                    // ✅ Handle string case (double serialization from SQL)
                    var bestOfferJson = bestOfferElement.GetString();
                    if (!string.IsNullOrWhiteSpace(bestOfferJson))
                    {
                        bestOffer = JsonSerializer.Deserialize<BestOffer>(bestOfferJson, JsonOptions);
                    }
                }
                else if (bestOfferElement.ValueKind == JsonValueKind.Object)
                {
                    // ✅ Handle object case (direct deserialization)
                    var bestOfferJson = bestOfferElement.GetRawText();
                    bestOffer = JsonSerializer.Deserialize<BestOffer>(bestOfferJson, JsonOptions);
                }
            }

            return new PricingDto
            {
                VendorCount = vendorCount,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                BestOffer = bestOffer == null
                    ? new BestPriceOfferDto()
                    : new BestPriceOfferDto
                    {
                        OfferId = bestOffer.OfferId,
                        OfferPricingId = bestOffer.OfferPricingId,
                        VendorId = bestOffer.VendorId,
                        VendorName = bestOffer.VendorName,
                        VendorRating = bestOffer.VendorRating ?? 0.0m,
                        Price = bestOffer.Price,
                        SalesPrice = bestOffer.SalesPrice,
                        DiscountPercentage = bestOffer.DiscountPercentage,
                        AvailableQuantity = bestOffer.AvailableQuantity,
                        StockStatus = bestOffer.StockStatus,
                        IsFreeShipping = bestOffer.IsFreeShipping,
                        EstimatedDeliveryDays = bestOffer.EstimatedDeliveryDays,
                        IsBuyBoxWinner = bestOffer.IsBuyBoxWinner,
                        MinOrderQuantity = bestOffer.MinOrderQuantity,
                        MaxOrderQuantity = bestOffer.MaxOrderQuantity,
                        QuantityTiers = bestOffer.QuantityTiers?.Select(qt => new QuantityTierDto
                        {
                            MinQuantity = qt.MinQuantity,
                            MaxQuantity = qt.MaxQuantity,
                            PricePerUnit = qt.PricePerUnit,
                            SalesPricePerUnit = qt.SalesPricePerUnit
                        }).ToList()
                    }
            };
        }
        catch (JsonException ex)
        {
            // Log the exception if you have a logger available
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}