using Domains.Entities.Offer;
using Domains.Views.Offer;
using Newtonsoft.Json;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Offer;

namespace BL.Mapper;

// Item mappings partial (MappingProfile.Items.cs)
public partial class MappingProfile
{
    private void ConfigureOfferMappings()
    {
        // Core item mappings
        CreateMap<TbOffer, OfferDto>()
            .ReverseMap();

        CreateMap<VwOffer, VwOfferDto>()
        .ForMember(dest => dest.ItemImages,
            opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<ItemImageDto>>(src.ItemImages ?? "[]")))
        .ForMember(dest => dest.ItemAttributes,
            opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<ItemAttributeDto>>(src.ItemAttributes ?? "[]")))
        .ForMember(dest => dest.OfferCombinations,
            opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<OfferCombinationPricingDetailsDto>>(src.OfferCombinationsJson ?? "[]")))
        .ReverseMap()
        .ForMember(dest => dest.ItemImages,
            opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ItemImages)))
        .ForMember(dest => dest.ItemAttributes,
            opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ItemAttributes)))
        .ForMember(dest => dest.OfferCombinationsJson,
            opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.OfferCombinations)));

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
