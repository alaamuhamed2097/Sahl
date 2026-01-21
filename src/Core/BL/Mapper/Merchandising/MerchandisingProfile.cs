using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Shared.DTOs.Campaign;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Mapper;

/// <summary>
/// AutoMapper Profile for MerchandisingProfile entities
/// </summary>
public partial class MappingProfile
{
    private void ConfigureMerchandisingMappings()
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

        // Category mapping - null-safe
        CreateMap<TbBlockCategory, CategoryCardDto>()
            .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src =>
                src.Category != null ? src.Category.TitleAr : null))
            .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src =>
                src.Category != null ? src.Category.TitleEn : null))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                src.Category != null ? src.Category.ImageUrl : null));

        // Item Combination to Product Card - null-safe with ThumbnailImage
        CreateMap<TbItemCombination, ItemCardDto>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src =>
                src.Item != null ? src.Item.Id : Guid.Empty))
            .ForMember(dest => dest.ItemCombinationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src =>
                src.Item != null ? src.Item.TitleAr : null))
            .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src =>
                src.Item != null ? src.Item.TitleEn : null))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src =>
                src.Item != null ? src.Item.AverageRating : 0))
            // Stock availability - null-safe
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                src.OfferCombinationPricings != null &&
                src.OfferCombinationPricings.Any(op => !op.IsDeleted && op.AvailableQuantity > 0)))
            .ForMember(dest => dest.InStock, opt => opt.MapFrom(src =>
                src.OfferCombinationPricings != null &&
                src.OfferCombinationPricings.Any(op => !op.IsDeleted && op.AvailableQuantity > 0)))
            // Main Image URL - using ThumbnailImage (null-safe)
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.Item != null ? src.Item.ThumbnailImage : null))
            .ForMember(dest => dest.CampaignBadgeAr, opt => opt.Ignore())
            .ForMember(dest => dest.CampaignBadgeEn, opt => opt.Ignore());

        // Campaign Item to Product Card - null-safe with ThumbnailImage
        CreateMap<TbCampaignItem, ItemCardDto>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null ? src.OfferCombinationPricing.ItemCombination.Item.Id : Guid.Empty))
            .ForMember(dest => dest.ItemCombinationId, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null && src.OfferCombinationPricing.ItemCombination.Item.ItemCombinations != null && src.OfferCombinationPricing.ItemCombination.Item.ItemCombinations.Any(c => c.IsDefault)
                    ? src.OfferCombinationPricing.ItemCombination.Item.ItemCombinations.Where(c => c.IsDefault).Select(c => c.Id).FirstOrDefault()
                    : Guid.Empty))
            .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null ? src.OfferCombinationPricing.ItemCombination.Item.TitleAr : null))
            .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null ? src.OfferCombinationPricing.ItemCombination.Item.TitleEn : null))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null ? src.OfferCombinationPricing.ItemCombination.Item.AverageRating : 0))
            // Main Image URL - using ThumbnailImage (null-safe)
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.OfferCombinationPricing.ItemCombination.Item != null ? src.OfferCombinationPricing.ItemCombination.Item.ThumbnailImage : null))
            .ForMember(dest => dest.CampaignBadgeAr, opt => opt.MapFrom(src =>
                src.Campaign != null ? src.Campaign.BadgeTextAr : null))
            .ForMember(dest => dest.CampaignBadgeEn, opt => opt.MapFrom(src =>
                src.Campaign != null ? src.Campaign.BadgeTextEn : null));

        // Campaign mappings - null-safe
        CreateMap<TbCampaign, CampaignDto>()
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src =>
                src.CampaignItems != null ? src.CampaignItems.Count(ci => !ci.IsDeleted) : 0));
          
        CreateMap<CreateCampaignDto, TbCampaign>();

        CreateMap<UpdateCampaignDto, TbCampaign>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't update ID
            .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

		// Campaign Item mappings - null-safe
		CreateMap<TbCampaignItem, CampaignItemDto>()
			.ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src =>
				src.OfferCombinationPricing != null &&
				src.OfferCombinationPricing.ItemCombination != null &&
				src.OfferCombinationPricing.ItemCombination.Item != null
					? src.OfferCombinationPricing.ItemCombination.Item.TitleAr
					: null))
			.ForMember(dest => dest.ItemNameEn, opt => opt.MapFrom(src =>
				src.OfferCombinationPricing != null &&
				src.OfferCombinationPricing.ItemCombination != null &&
				src.OfferCombinationPricing.ItemCombination.Item != null
					? src.OfferCombinationPricing.ItemCombination.Item.TitleEn
					: null))
			.ForMember(dest => dest.ItemImageUrl, opt => opt.MapFrom(src =>
				src.OfferCombinationPricing != null &&
				src.OfferCombinationPricing.ItemCombination != null &&
				src.OfferCombinationPricing.ItemCombination.Item != null
					? src.OfferCombinationPricing.ItemCombination.Item.ThumbnailImage
					: null));

		// Mapping for Add DTO
		CreateMap<AddCampaignItemDto, TbCampaignItem>()
			.ForMember(dest => dest.OfferCombinationPricingId, opt => opt.MapFrom(src => src.OfferCompinationPriceId))
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
			.ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

		CreateMap<UpdateCampaignItemDto, TbCampaignItem>()
		   .ForMember(dest => dest.Id, opt => opt.Ignore())
		   .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
		   .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
		   .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
		   .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore())
		   .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
	}
}