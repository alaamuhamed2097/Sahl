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
        ConfigureNotificationChannelMappings();
        ConfigurePricingMappings();
        ConfigureCartMappings();
        ConfigureCustomerAddressMapping();
        ConfigureHomePageSliderMappingProfile();
        ConfigureRefundMappings();
        ConfigureCustomerMappings();
        ConfigureWithdrawalMethodsMappings();
        ConfigurePaymentMethodMappings();
        ConfigureWalletMappings();

	}
}
