namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Services
{
    using Domain;

    public interface ILogEventIndexerService
    {
        void Index(string logEvent);
    }
}