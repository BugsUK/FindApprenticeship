namespace SFA.Apprenticeships.Application.Interfaces
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}