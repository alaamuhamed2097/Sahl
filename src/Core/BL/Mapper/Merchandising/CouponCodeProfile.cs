using Domains.Entities.Merchandising.CouponCode;
using Shared.DTOs.Order.CouponCode;

namespace BL.Mapper;

/// <summary>
/// AutoMapper Profile for CouponCodeProfile entities
/// </summary>
public partial class MappingProfile
{
    private void ConfigureCouponCodeMappings()
    {
        // TbCouponCode <-> CouponCodeDto
        CreateMap<TbCouponCode, CouponCodeDto>()
            .ForMember(
                dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.StoreName : null))
            .ForMember(
                dest => dest.ScopeItems,
                opt => opt.MapFrom(src => src.CouponScopes));

        CreateMap<CouponCodeDto, TbCouponCode>()
            .ForMember(dest => dest.Vendor, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.CouponScopes, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<TbCouponCodeScope, CouponScopeDto>()
            .ReverseMap()
            .ForMember(dest => dest.CouponCode, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore());
    }
}