using AutoMapper;
using BL.Contracts.IMapper;

namespace BL.Mapper.Base;

public class BaseMapper : IBaseMapper
{
    private readonly IMapper _mapper;

    public BaseMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination MapModel<TSource, TDestination>(TSource source)
    {
        try
        {
            return _mapper.Map<TDestination>(source);
        }
        catch (AutoMapperMappingException ex)
        {
            throw new Exception($"Mapping error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred during mapping: {ex.Message}", ex);
        }
    }

    public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
    {
        try
        {
            return _mapper.Map<IEnumerable<TDestination>>(source);
        }
        catch (AutoMapperMappingException ex)
        {
            throw new Exception($"Mapping error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred during mapping: {ex.Message}", ex);
        }
    }
}
