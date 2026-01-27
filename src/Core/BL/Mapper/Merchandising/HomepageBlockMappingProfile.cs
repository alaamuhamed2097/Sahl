using AutoMapper;
using Common.Enumerations.Merchandising;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Mapper.Merchandising
{
    /// <summary>
    /// Mapping profile for Homepage Block DTOs
    /// </summary>
    public class HomepageBlockMappingProfile : Profile
    {
        public HomepageBlockMappingProfile()
        {
            // TbHomepageBlock to AdminBlockListDto
            CreateMap<TbHomepageBlock, AdminBlockListDto>()
                .ForMember(dest => dest.StatusBadge, opt => opt.Ignore())
                .ReverseMap();

            // TbHomepageBlock to AdminBlockCreateDto
            CreateMap<TbHomepageBlock, AdminBlockCreateDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BlockItems))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.BlockCategories))
                .ReverseMap()
                .ForMember(dest => dest.BlockItems, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    // Only map items if Type is ManualItems
                    if (!string.IsNullOrEmpty(src.Type) && 
                        (src.Type.Equals("ManualItems", StringComparison.OrdinalIgnoreCase) || 
                         src.Type == "1") && 
                        src.Items != null && src.Items.Any())
                    {
                        return src.Items.Select(item => new TbBlockItem
                        {
                            ItemId = item.ItemId,
                            DisplayOrder = item.DisplayOrder,
                            HomepageBlockId = dest.Id
                        }).ToList();
                    }
                    return new List<TbBlockItem>();
                }))
                .ForMember(dest => dest.BlockCategories, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    // Only map categories if Type is ManualCategories
                    if (!string.IsNullOrEmpty(src.Type) && 
                        (src.Type.Equals("ManualCategories", StringComparison.OrdinalIgnoreCase) || 
                         src.Type == "2") && 
                        src.Categories != null && src.Categories.Any())
                    {
                        return src.Categories.Select(cat => new TbBlockCategory
                        {
                            CategoryId = cat.CategoryId,
                            DisplayOrder = cat.DisplayOrder,
                            HomepageBlockId = dest.Id
                        }).ToList();
                    }
                    return new List<TbBlockCategory>();
                }));

            // TbBlockItem to AdminBlockItemDto
            CreateMap<TbBlockItem, AdminBlockItemDto>()
                .ForMember(dest => dest.ItemNameEn, opt => opt.MapFrom(src => src.Item != null ? src.Item.TitleEn : string.Empty))
                .ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src => src.Item != null ? src.Item.TitleAr : string.Empty))
                .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.Item != null ? src.Item.ThumbnailImage : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item != null ? src.Item.BasePrice : 0))
                .ReverseMap();

            // TbBlockCategory to AdminBlockCategoryDto
            CreateMap<TbBlockCategory, AdminBlockCategoryDto>()
                .ForMember(dest => dest.CategoryNameEn, opt => opt.MapFrom(src => src.Category != null ? src.Category.TitleEn : string.Empty))
                .ForMember(dest => dest.CategoryNameAr, opt => opt.MapFrom(src => src.Category != null ? src.Category.TitleAr : string.Empty))
                .ForMember(dest => dest.CategoryImage, opt => opt.MapFrom(src => src.Category != null ? src.Category.ImageUrl : null))
                .ReverseMap();

            // Block Reorder DTO
            CreateMap<TbHomepageBlock, BlockReorderDto>()
                .ForMember(dest => dest.BlockId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NewDisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder));
        }
    }
}
