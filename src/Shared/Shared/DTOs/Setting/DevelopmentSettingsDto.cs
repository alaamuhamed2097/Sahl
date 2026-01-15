using Shared.DTOs.Base;

namespace Shared.DTOs.Setting
{
    public class DevelopmentSettingsDto : BaseDto
    {
        public bool IsMultiVendorSystem { get; set; } = true;
    }
}