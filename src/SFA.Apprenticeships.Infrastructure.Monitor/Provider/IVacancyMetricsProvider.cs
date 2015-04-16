namespace SFA.Apprenticeships.Infrastructure.Monitor.Provider
{
    public interface IVacancyMetricsProvider
    {
        long GetApprenticeshipsCount();

        long GetTraineeshipsCount();
    }
}