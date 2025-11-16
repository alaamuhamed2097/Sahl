using AutoMapper;

namespace BL.Mapper
{
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
            ConfigureCurrencyMappings();
            ConfigureBrandMapping();
            ConfigureMediaMappings();
            ConfigureOrderMappings();
            ConfigureSettingMappings();
            ConfigureVendorMappings();
            ConfigureCustomerMappings();

		}
    }
}
