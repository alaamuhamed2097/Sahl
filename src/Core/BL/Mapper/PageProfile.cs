using AutoMapper;
using Shared.DTOs.Page;

namespace BL.Mapper
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<TbPage, PageDto>().ReverseMap();
        }
    }
}