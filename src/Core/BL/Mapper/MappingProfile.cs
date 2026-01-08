using AutoMapper;

namespace BL.Mapper;

// Main mapping profile file (MappingProfile.cs)
public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureUserMappings();
        ConfigureNotificationMappings();
        ConfigureCategoryMappings();
        ConfigureAttributeMappings();
        ConfigureUnitMappings();
        ConfigureItemMappings();
        ConfigureMerchandisingMappings();
        ConfigureCouponCodeMappings();
        ConfigureOfferMappings();
        ConfigureCurrencyMappings();
        ConfigureBrandMapping();
        ConfigureMediaMappings();
        ConfigureOrderMappings();
        ConfigureSettingMappings();
        ConfigureVendorMappings();
        ConfigureSupportMappings();
        ConfigureReviewMappings();
        ConfigureWarehouseAndInventoryMappings();
        ConfigureContentAndNotificationChannelMappings();
        ConfigurePricingMappings();
        ConfigureCartMappings();
        ConfigureCustomerAddressMapping();
        ConfigureHomePageSliderMappingProfile();
        ConfigureRefundMappings();
        ConfigureCustomerMappings();
        ConfigureWithdrawalMethodsMappings();
    }
}
