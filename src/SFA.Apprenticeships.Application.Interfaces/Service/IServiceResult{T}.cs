namespace SFA.Apprenticeships.Application.Interfaces.Service
{
    public interface IServiceResult<T> : IServiceResult
    {
        T Result { get; }
    }
}