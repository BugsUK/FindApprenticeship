namespace SFA.Apprenticeships.Application.Applications
{
    using Application.Entities;

    public interface IApplicationStatusProcessor
    {
        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary, bool strictEtlValidation);

        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
