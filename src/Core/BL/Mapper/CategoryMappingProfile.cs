using Domins.Entities.Category;
using Domins.Views.Category;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Category mappings partial (MappingProfile.Categories.cs)
    public partial class MappingProfile
    {
        private void ConfigureCategoryMappings()
        {
            // Category main mappings


            CreateMap<TbCategory, CategoryDto>()
            .ReverseMap()
            .ForMember(dest => dest.CategoryAttributes, opt => opt.Ignore());

            // Category attributes
            CreateMap<TbCategoryAttribute, CategoryAttributeDto>()
                .ForMember(dest => dest.AttributeOptions, opt => opt.Ignore())
                .ReverseMap();

            // Category with attributes
            CreateMap<VwCategoryWithAttributes, CategoryDto>()
                 .ForMember(dest => dest.CategoryAttributes, opt => opt.MapFrom(src =>
                     !string.IsNullOrEmpty(src.AttributesJson)
                         ? (JsonSerializer.Deserialize<List<CategoryAttributeDto>>(src.AttributesJson,
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = true,
                                 DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                             }) ?? new List<CategoryAttributeDto>())
                             .OrderBy(a => a.DisplayOrder)
                             .ToList()
                         : new List<CategoryAttributeDto>()));


            // Category with items
            CreateMap<VwCategoryItems, VwCategoryItemsDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ItemsJson)
                        ? JsonSerializer.Deserialize<List<ItemSectionDto>>(src.ItemsJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        : new List<ItemSectionDto>()));

            // Category preview mappings
            CreateMap<TbCategory, CategoryPreviewDto>().ReverseMap();
            CreateMap<VwCategoryWithAttributes, CategoryPreviewDto>()
                .ForMember(dest => dest.CategoryAttributes, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.AttributesJson)
                        ? (JsonSerializer.Deserialize<List<CategoryAttributeDto>>(src.AttributesJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            }) ?? new List<CategoryAttributeDto>())
                             .OrderBy(a => a.DisplayOrder)
                             .ToList()
                        : new List<CategoryAttributeDto>()));

            CreateMap<TbCategory, MainCategoryDto>().ReverseMap();
        }
    }
}
