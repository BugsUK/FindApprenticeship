namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Service;

    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {
        public ServiceResult(string code, T result) : base(code)
        {
            Result = result;
        }

        public T Result { get; }
    }
}