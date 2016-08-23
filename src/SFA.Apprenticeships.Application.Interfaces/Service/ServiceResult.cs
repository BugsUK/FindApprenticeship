namespace SFA.Apprenticeships.Application.Interfaces.Service
{
    public class ServiceResult : IServiceResult
    {
        public ServiceResult(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}