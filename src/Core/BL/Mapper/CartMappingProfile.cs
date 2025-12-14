using Domains.Entities.ECommerceSystem.Cart;
using Shared.DTOs.ECommerce.Cart;

namespace BL.Mapper
{
    /// <summary>
    /// Cart and Shopping related mappings
    /// </summary>
    public partial class MappingProfile
    {
        private void ConfigureCartMappings()
        {
            // TbShoppingCart -> CartSummaryDto (done manually in CartService for complex calculations)
            // TbShoppingCartItem -> CartItemDto (done manually in CartService for includes)

            // Basic mappings can be added here if needed
            CreateMap<TbShoppingCart, CartSummaryDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.Ignore())  // Handled in service
                .ForMember(dest => dest.SubTotal, opt => opt.Ignore())  // Calculated in service
                .ForMember(dest => dest.ShippingEstimate, opt => opt.Ignore())  // Calculated in service
                .ForMember(dest => dest.TaxEstimate, opt => opt.Ignore())  // Calculated in service
                .ForMember(dest => dest.TotalEstimate, opt => opt.Ignore())  // Calculated in service
                .ForMember(dest => dest.ItemCount, opt => opt.Ignore());  // Calculated in service

            CreateMap<TbShoppingCartItem, CartItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.TitleEn))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.OfferCombinationPricing.Offer.Vendor.CompanyName))
                .ForMember(dest => dest.SubTotal, opt => opt.Ignore());  // Calculated in service
        }
    }
}
