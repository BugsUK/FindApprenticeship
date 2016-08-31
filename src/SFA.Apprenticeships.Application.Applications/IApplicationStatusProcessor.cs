namespace SFA.Apprenticeships.Application.Applications
{
    using Application.Entities;
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatusesPages(int applicationStatusExtractWindow);

        void QueueApplicationStatuses(int applicationStatusExtractWindow, ApplicationUpdatePage applicationStatusSummaryPage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary, bool strictEtlValidation);

        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
