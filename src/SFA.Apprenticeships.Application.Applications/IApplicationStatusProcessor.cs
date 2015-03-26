namespace SFA.Apprenticeships.Application.Applications
{
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatusesPages(int applicationStatusExtractWindow);

        void QueueApplicationStatuses(int applicationStatusExtractWindow, ApplicationUpdatePage applicationStatusSummaryPage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);

        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
