using AutoMapper;
using Domains.Entities.Page;
using Shared.DTOs.Page;

namespace BL.MappingProfiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<TbPage, PageDto>().ReverseMap();
        }
    }
}