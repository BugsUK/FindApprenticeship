namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public static class ProviderVacancyStatusPresenter
    {
        public static bool IsStateReadOnly(this ProviderVacancyStatuses status)
        {
            return status == ProviderVacancyStatuses.PendingQA || status == ProviderVacancyStatuses.Live ||
                   status == ProviderVacancyStatuses.ReservedForQA || status == ProviderVacancyStatuses.Closed;
        }

        public static bool IsStateEditable(this ProviderVacancyStatuses status)
        {
            return status == ProviderVacancyStatuses.Unknown || status == ProviderVacancyStatuses.Draft ||
                   status == ProviderVacancyStatuses.RejectedByQA;
        }

        public static bool IsStateReviewable(this ProviderVacancyStatuses status)
        {
            return status == ProviderVacancyStatuses.PendingQA || status == ProviderVacancyStatuses.ReservedForQA || 
                   status == ProviderVacancyStatuses.RejectedByQA;
        }

        public static bool IsStateInQa(this ProviderVacancyStatuses status)
        {
            return status == ProviderVacancyStatuses.PendingQA || status == ProviderVacancyStatuses.ReservedForQA;
        }

        public static bool CanHaveApplications(this ProviderVacancyStatuses status)
        {
            return status == ProviderVacancyStatuses.Live || status == ProviderVacancyStatuses.Closed ||
                   status == ProviderVacancyStatuses.Completed || status == ProviderVacancyStatuses.Withdrawn;
        }
    }
}