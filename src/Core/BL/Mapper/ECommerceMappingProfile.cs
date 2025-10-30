using Domins.Entities.Category;
using Domins.Views;
using Domins.Views.Category;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.Category;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureECommerceMapping()
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


            //// Category with items
            //CreateMap<VwCategoryItems, VwCategoryItemsDto>()
            //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
            //        !string.IsNullOrEmpty(src.ItemsJson)
            //            ? JsonSerializer.Deserialize<List<ItemSectionDto>>(src.ItemsJson,
            //                new JsonSerializerOptions
            //                {
            //                    PropertyNameCaseInsensitive = true,
            //                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            //                })
            //            : new List<ItemSectionDto>()));

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


            // Attribute main mappings
            CreateMap<TbAttribute, AttributeDto>()
                .ReverseMap()
                 .ForMember(dest => dest.AttributeOptions, opt => opt.Ignore());

            // Attribute options
            CreateMap<TbAttributeOption, AttributeOptionDto>()
                .ReverseMap();
            // Attribute with options
            CreateMap<VwAttributeWithOptions, AttributeDto>()
                .ForMember<List<AttributeOptionDto>>(dest => dest.AttributeOptions, opt => opt.MapFrom<List<AttributeOptionDto>>(src =>
                    !string.IsNullOrEmpty(src.AttributeOptionsJson)
                        ? JsonSerializer.Deserialize<List<AttributeOptionDto>>(src.AttributeOptionsJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            }) ?? new List<AttributeOptionDto>()
                            .OrderBy(o => o.DisplayOrder).ToList()
                        : new List<AttributeOptionDto>()));


            //CreateMap<VwItemWithAttributes, ItemAttributeDetailsDto>().ReverseMap();
        }
    }
}
