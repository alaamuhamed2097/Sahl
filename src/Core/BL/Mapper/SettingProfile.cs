using Shared.DTOs.Setting;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureSettingMappings()
        {
            CreateMap<TbSetting, SettingDto>().ReverseMap();
        }
    }
}