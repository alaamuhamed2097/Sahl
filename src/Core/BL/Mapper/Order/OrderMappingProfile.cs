using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order;
using Domains.Entities.Order.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureOrderMappings()
    {
        CreateMap<TbOrder, OrderDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.User.ProfileImagePath))
            .ReverseMap();
        CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
        CreateMap<TbCouponCode, CouponCodeDto>().ReverseMap();

        // Shipment Mappings
        CreateMap<TbOrderShipment, ShipmentDto>()
            .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor != null && src.Vendor.User != null ? src.Vendor.User.FirstName + " " + src.Vendor.User.LastName : string.Empty))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Address : string.Empty))
            .ForMember(dest => dest.ShippingCompanyName, opt => opt.MapFrom(src => src.ShippingCompany != null ? src.ShippingCompany.Name : string.Empty))
            .ReverseMap();

        CreateMap<TbOrderShipmentItem, ShipmentItemDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.TitleAr + " - " + src.Item.TitleEn : string.Empty))
            .ForMember(dest => dest.TitleAr, opt => opt.MapFrom(src => src.Item != null ? src.Item.TitleAr : string.Empty))
            .ForMember(dest => dest.TitleEn, opt => opt.MapFrom(src => src.Item != null ? src.Item.TitleEn : string.Empty))
            .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.Item != null ? src.Item.ThumbnailImage : string.Empty))
            .ReverseMap();

        CreateMap<TbShipmentStatusHistory, ShipmentStatusHistoryDto>().ReverseMap();
    }
}
