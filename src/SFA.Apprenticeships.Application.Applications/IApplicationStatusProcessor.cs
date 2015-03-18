namespace SFA.Apprenticeships.Application.Applications
{
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatusesPages();

        void QueueApplicationStatuses(ApplicationUpdatePage applicationStatusSummaryPage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);

        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
