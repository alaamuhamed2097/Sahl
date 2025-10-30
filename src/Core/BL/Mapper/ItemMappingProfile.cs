using Domins.Entities.Item;
using Domins.Views.Item;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Item mappings partial (MappingProfile.Items.cs)
    public partial class MappingProfile
    {
        private void ConfigureItemMappings()
        {
            // Core item mappings
            CreateMap<TbItem, ItemDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ItemImages))
                .ForMember(dest => dest.CategoryTitleAr, opt => opt.MapFrom(src => src.Category.TitleAr))
                .ForMember(dest => dest.CategoryTitleEn, opt => opt.MapFrom(src => src.Category.TitleEn))
                .ReverseMap();

            CreateMap<TbItem, Item>()
                .ReverseMap();

            CreateMap<TbItemImage, ItemImageViewDto>()
                .ReverseMap();

            CreateMap<VwItem, VwItemDto>()
                .ForMember(dest => dest.ItemImages, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.ItemImagesJson)
                        ? new List<ItemImageViewDto>()
                        : DeserializeItemImages(src.ItemImagesJson)))
                .ReverseMap();

            // Item attribute mappings
            CreateMap<TbItemAttribute, ItemAttributeDto>()
                .ReverseMap();

            // Item pricing combinations
            CreateMap<TbItemAttributeCombinationPricing, ItemAttributeCombinationPricingDto>()
                .ReverseMap();

            // Item images
            CreateMap<TbItemImage, ItemImageDto>()
                .ReverseMap();
        }

        private static List<ItemImageViewDto> DeserializeItemImages(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = null, // Don't convert property names
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                return JsonSerializer.Deserialize<List<ItemImageViewDto>>(json, options) ?? new List<ItemImageViewDto>();
            }
            catch (JsonException ex)
            {
                // You might want to log this exception
                // For now, return empty list
                return new List<ItemImageViewDto>();
            }
        }
    }
}
