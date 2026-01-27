using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Order.Cart;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Order.Cart;

namespace BL.Mapper;

/// <summary>
/// Cart and Shopping related mappings
/// </summary>
public partial class MappingProfile
{
    private void ConfigureCartMappings()
    {
		// TbShoppingCart -> CartSummaryDto (done manually in CartService for complex calculations)
		// TbShoppingCartItem -> CartItemDto (done manually in CartService for includes)

		//// Basic mappings can be added here if needed
		//CreateMap<TbShoppingCart, CartSummaryDto>()
		//    .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
		//    .ForMember(dest => dest.Items, opt => opt.Ignore())  // Handled in service
		//    .ForMember(dest => dest.SubTotal, opt => opt.Ignore())  // Calculated in service
		//    .ForMember(dest => dest.ShippingEstimate, opt => opt.Ignore())  // Calculated in service
		//    .ForMember(dest => dest.TaxEstimate, opt => opt.Ignore())  // Calculated in service
		//    .ForMember(dest => dest.TotalEstimate, opt => opt.Ignore())  // Calculated in service
		//    .ForMember(dest => dest.ItemCount, opt => opt.Ignore());  // Calculated in service

		//CreateMap<TbShoppingCartItem, CartItemDto>()
		//    .ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src => src.Item.TitleAr))
		//    .ForMember(dest => dest.ItemNameEn, opt => opt.MapFrom(src => src.Item.TitleEn))
		//    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.OfferCombinationPricing.Offer.Vendor.StoreName))
		//    .ForMember(dest => dest.SubTotal, opt => opt.Ignore());  // Calculated in service

		// TbShoppingCart -> CartSummaryDto
		CreateMap<TbShoppingCart, CartSummaryDto>()
			.ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Items, opt => opt.Ignore())
			.ForMember(dest => dest.SubTotal, opt => opt.Ignore())
			.ForMember(dest => dest.ShippingEstimate, opt => opt.Ignore())
			.ForMember(dest => dest.TaxEstimate, opt => opt.Ignore())
			.ForMember(dest => dest.TotalEstimate, opt => opt.Ignore())
			.ForMember(dest => dest.ItemCount, opt => opt.Ignore());

		// TbShoppingCartItem -> CartItemDto
		CreateMap<TbShoppingCartItem, CartItemDto>()
			.ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src => src.Item.TitleAr))
			.ForMember(dest => dest.ItemNameEn, opt => opt.MapFrom(src => src.Item.TitleEn))
			.ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.OfferCombinationPricing.Offer.Vendor.StoreName))
			.ForMember(dest => dest.UnitOriginalPrice, opt => opt.MapFrom(src => src.OfferCombinationPricing.Price))
			.ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.OfferCombinationPricing.AvailableQuantity > 0))
			.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Item.ThumbnailImage))
			.ForMember(dest => dest.SubTotal, opt => opt.Ignore())  // Calculated in service
			.ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Item.ItemAttributes.Where(a => !a.IsDeleted)));

		// TbItemAttribute -> CartItemAttributeDto (النسخة المبسطة)
		CreateMap<TbItemAttribute, AttributeFilterDto>()
			.ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.AttributeId))
			.ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.TitleAr))
			.ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.TitleEn))
			.ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Value))
			.ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder));

	}
}
