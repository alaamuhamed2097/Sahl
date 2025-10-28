using AutoMapper;
using Domains.Entities.Setting;
using Shared.DTOs.Setting;

namespace BL.MappingProfiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<TbSetting, SettingDto>().ReverseMap();
        }
    }
}