namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Service;

    public class ServiceResult : IServiceResult
    {
        public ServiceResult(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}