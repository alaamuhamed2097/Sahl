using AutoMapper;
using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Merchandising;
using Shared.DTOs.Campaign;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Mapper
{
    /// <summary>
    /// AutoMapper Profile for MerchandisingProfile entities
    /// </summary>
    public class MerchandisingProfile : Profile
    {
        public MerchandisingProfile()
        {
            // Block mapping
            CreateMap<TbHomepageBlock, HomepageBlockDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout.ToString()))
                .ForMember(dest => dest.Campaign, opt => opt.MapFrom(src => src.Campaign))
                .ForMember(dest => dest.Products, opt => opt.Ignore()) // Will be populated in service
                .ForMember(dest => dest.Categories, opt => opt.Ignore()); // Will be populated in service

            // Campaign mapping
            CreateMap<TbCampaign, CampaignInfoDto>();

            // Category mapping
            CreateMap<TbBlockCategory, CategoryCardDto>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.Category.TitleAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Category.TitleEn))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Category.ImageUrl)); // ✅ Fixed

            // Item Combination to Product Card
            CreateMap<TbItemCombination, ItemCardDto>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item.Id))
                .ForMember(dest => dest.ItemCombinationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.Item.TitleAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Item.TitleEn))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Item.AverageRating))
                // Stock availability - check from OfferCombinationPricing
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                    src.OfferCombinationPricings.Any(op => !op.IsDeleted && op.AvailableQuantity > 0)))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src =>
                    src.OfferCombinationPricings.Any(op => !op.IsDeleted && op.AvailableQuantity > 0)))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.ItemCombinationImages.OrderBy(i => i.Order).FirstOrDefault().Path ??
                    src.Item.ItemImages.OrderBy(i => i.Order).FirstOrDefault().Path))
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Will be set by pricing service
                .ForMember(dest => dest.SalePrice, opt => opt.Ignore()) // Will be set by pricing service
                .ForMember(dest => dest.DiscountPercentage, opt => opt.Ignore()) // Will be set by pricing service
                .ForMember(dest => dest.CampaignBadgeAr, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignBadgeEn, opt => opt.Ignore());

            // Campaign Item to Product Card
            CreateMap<TbCampaignItem, ItemCardDto>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item.Id))
                .ForMember(dest => dest.ItemCombinationId, opt => opt.MapFrom(src =>
                    src.Item.ItemCombinations.FirstOrDefault(c => c.IsDefault).Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.Item.TitleAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Item.TitleEn))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Item.AverageRating))
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Will be calculated from combination pricing
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.CampaignPrice))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.Ignore()) // Will be calculated in service
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                    !src.StockLimit.HasValue || (src.StockLimit.Value - src.SoldCount) > 0))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src =>
                    !src.StockLimit.HasValue || (src.StockLimit.Value - src.SoldCount) > 0))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.Item.ItemCombinations.FirstOrDefault(c => c.IsDefault).ItemCombinationImages
                        .OrderBy(i => i.Order).FirstOrDefault().Path ??
                    src.Item.ItemImages.OrderBy(i => i.Order).FirstOrDefault().Path))
                .ForMember(dest => dest.CampaignBadgeAr, opt => opt.MapFrom(src => src.Campaign.BadgeTextAr))
                .ForMember(dest => dest.CampaignBadgeEn, opt => opt.MapFrom(src => src.Campaign.BadgeTextEn));

            // Campaign mappings
            CreateMap<TbCampaign, CampaignDto>()
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src =>
                    src.CampaignItems != null ? src.CampaignItems.Count(ci => !ci.IsDeleted) : 0))
                .ForMember(dest => dest.TotalSold, opt => opt.MapFrom(src =>
                    src.CampaignItems != null ? src.CampaignItems.Where(ci => !ci.IsDeleted).Sum(ci => ci.SoldCount) : 0));

            CreateMap<CreateCampaignDto, TbCampaign>();

            CreateMap<UpdateCampaignDto, TbCampaign>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't update ID
                .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            // Campaign Item mappings
            CreateMap<TbCampaignItem, CampaignItemDto>()
                .ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src => src.Item.TitleAr))
                .ForMember(dest => dest.ItemNameEn, opt => opt.MapFrom(src => src.Item.TitleEn))
                .ForMember(dest => dest.ItemImageUrl, opt => opt.MapFrom(src =>
                    src.Item.ItemImages.OrderBy(i => i.Order).FirstOrDefault().Path));

            CreateMap<AddCampaignItemDto, TbCampaignItem>();
        }
    }
}
