using Domains.Entities.Order;
using Shared.DTOs.Order.Checkout.Address;

namespace BL.Mapper;

/// <summary>
/// AutoMapper profile for CustomerAddress entity mappings
/// Handles conversions between TbCustomerAddress entity and various DTOs
/// </summary>
public partial class MappingProfile
{
    private void ConfigureCustomerAddressMapping()
    {
        // ============================================================
        // Entity to DTO Mappings
        // ============================================================

        // TbCustomerAddress → CustomerAddressDto
        CreateMap<TbCustomerAddress, CustomerAddressDto>()
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDateUtc))
            .ForMember(dest => dest.CityName,
                opt => opt.MapFrom(src => src.City.TitleAr))
            .ForMember(dest => dest.StateName,
                opt => opt.MapFrom(src => src.City.State.TitleAr))
            .ForMember(dest => dest.CountryName,
                opt => opt.MapFrom(src => src.City.State.Country.TitleAr));

        // TbCustomerAddress → AddressSelectionDto
        CreateMap<TbCustomerAddress, AddressSelectionDto>()
            .ForMember(dest => dest.AddressId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullPhoneNumber,
                opt => opt.MapFrom(src => src.PhoneCode + src.PhoneNumber))
            .ForMember(dest => dest.FullAddress,
                opt => opt.Ignore()); // Set manually with city/state/country names

        // ============================================================
        // DTO to Entity Mappings
        // ============================================================

        // CreateCustomerAddressRequest → TbCustomerAddress
        CreateMap<CreateCustomerAddressRequest, TbCustomerAddress>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.UserId,
                opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.RecipientName,
                opt => opt.MapFrom(src => src.RecipientName.Trim()))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
            .ForMember(dest => dest.PhoneCode,
                opt => opt.MapFrom(src => src.PhoneCode.Trim()))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address.Trim()))
            .ForMember(dest => dest.IsDefault,
                opt => opt.Ignore()) // Handled by service logic
            .ForMember(dest => dest.IsDeleted,
                opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedDateUtc,
                opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.CreatedBy,
                opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.UpdatedDateUtc,
                opt => opt.Ignore())
            .ForMember(dest => dest.User,
                opt => opt.Ignore()); // Navigation property

        // UpdateCustomerAddressRequest → TbCustomerAddress
        CreateMap<UpdateCustomerAddressRequest, TbCustomerAddress>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore()) // Don't update ID
            .ForMember(dest => dest.UserId,
                opt => opt.Ignore()) // Don't update UserId
            .ForMember(dest => dest.RecipientName,
                opt => opt.MapFrom(src => src.RecipientName.Trim()))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
            .ForMember(dest => dest.PhoneCode,
                opt => opt.MapFrom(src => src.PhoneCode.Trim()))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address.Trim()))
            .ForMember(dest => dest.IsDefault,
                opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.IsDeleted,
                opt => opt.Ignore()) // Don't update through update request
            .ForMember(dest => dest.CreatedDateUtc,
                opt => opt.Ignore()) // Don't update creation date
            .ForMember(dest => dest.CreatedBy,
                opt => opt.Ignore()) // Don't update creator
            .ForMember(dest => dest.UpdatedDateUtc,
                opt => opt.Ignore()) // Set in service
            .ForMember(dest => dest.User,
                opt => opt.Ignore()); // Navigation property

        // InlineAddressDto → TbCustomerAddress (for quick checkout)
        CreateMap<InlineAddressDto, TbCustomerAddress>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.UserId,
                opt => opt.Ignore())
            .ForMember(dest => dest.RecipientName,
                opt => opt.MapFrom(src => src.RecipientName.Trim()))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
            .ForMember(dest => dest.PhoneCode,
                opt => opt.MapFrom(src => src.PhoneCode.Trim()))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address.Trim()))
            .ForMember(dest => dest.IsDefault,
                opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted,
                opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedDateUtc,
                opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,
                opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDateUtc,
                opt => opt.Ignore())
            .ForMember(dest => dest.User,
                opt => opt.Ignore());
    }
}