namespace SFA.Infrastructure.Interfaces
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}