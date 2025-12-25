using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using Domains.Views.Item;
using Domains.Views.Offer;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Item mappings partial (MappingProfile.Items.cs)
    public partial class MappingProfile
    {
        private void ConfigureOfferMappings()
        {
            // Core item mappings
            CreateMap<TbOffer, OfferDto>()
                .ReverseMap();

            CreateMap<VwOffer, VwOfferDto>()
            .ForMember(dest => dest.ItemImages, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.ItemImages)
                    ? JsonSerializer.Deserialize<List<ItemImageDto>>(src.ItemImages,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<ItemImageDto>()
                    : new List<ItemImageDto>()))
            .ForMember(dest => dest.ItemAttributes, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.ItemAttributes)
                    ? JsonSerializer.Deserialize<List<ItemAttributeDto>>(src.ItemAttributes,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<ItemAttributeDto>()
                    : new List<ItemAttributeDto>()))
            .ForMember(dest => dest.OfferCombinations, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.OfferCombinationsJson)
                    ? JsonSerializer.Deserialize<List<OfferCombinationPricingDetailsDto>>(src.OfferCombinationsJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<OfferCombinationPricingDetailsDto>()
                    : new List<OfferCombinationPricingDetailsDto>()))
            .ReverseMap();

            CreateMap<VwVendorItem, VendorItemDetailsDto>()
                .ForMember(dest => dest.BaseItemImages, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.BaseItemImages)
                    ? JsonSerializer.Deserialize<List<ImageDto>>(src.BaseItemImages,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<ImageDto>()
                    : new List<ImageDto>()))
                .ForMember(dest => dest.ItemCombinationImages, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.ItemCombinationImages)
                    ? JsonSerializer.Deserialize<List<ImageDto>>(src.ItemCombinationImages,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<ImageDto>()
                    : new List<ImageDto>()))
                .ForMember(dest => dest.ConbinationAttributes, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.ConbinationAttributes)
                    ? JsonSerializer.Deserialize<List<PricingAttributeDto>>(src.ConbinationAttributes,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }) ?? new List<PricingAttributeDto>()
                    : new List<PricingAttributeDto>()))
                .ReverseMap();

            // Additional offer mappings
            CreateMap<TbOfferCombinationPricing, OfferCombinationPricingDto>()
                .ReverseMap();
            CreateMap<TbOfferCondition, OfferConditionDto>()
                .ReverseMap();
            CreateMap<TbOfferPriceHistory, OfferPriceHistoryDto>()
                .ReverseMap();
            CreateMap<TbOfferStatusHistory, OfferStatusHistoryDto>()
                .ReverseMap();
        }
    }
}
