namespace BL.Contracts.IMapper
{
    public interface IBaseMapper
    {
        TDestination MapModel<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
