namespace SFA.Apprenticeships.Infrastructure.Log.Services
{
    using Domain;

    public interface ILogEventIndexerService
    {
        void Index(string logEvent);
    }
}
