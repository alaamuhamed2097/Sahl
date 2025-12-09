using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Views.Item;
using Shared.DTOs.ECommerce.Category;
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

            // Item combination mapping
            CreateMap<TbItemCombination, ItemCombinationDto>()
                .ReverseMap();

            // Item attribute mappings
            CreateMap<TbItemAttribute, ItemAttributeDto>()
                .ReverseMap();
            CreateMap<TbAttributeValuePriceModifier, AttributeValuePriceModifierDto>()
                .ReverseMap();

            // Combination attribute mappings
            CreateMap<TbCombinationAttribute, CombinationAttributeDto>()
                .ReverseMap();

            // Combination attribute value mappings
            CreateMap<TbCombinationAttributesValue, CombinationAttributeValueDto>()
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
    }
}
