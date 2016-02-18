namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Raa.Vacancies;

    public static class ProviderVacancyStatusPresenter
    {
        public static bool IsStateReadOnly(this VacancyStatus status)
        {
            return status == VacancyStatus.PendingQA || status == VacancyStatus.Live ||
                   status == VacancyStatus.ReservedForQA || status == VacancyStatus.Closed;
        }

        public static bool IsStateEditable(this VacancyStatus status)
        {
            return status == VacancyStatus.Unknown || status == VacancyStatus.Draft ||
                   status == VacancyStatus.RejectedByQA;
        }

        public static bool IsStateReviewable(this VacancyStatus status)
        {
            return status == VacancyStatus.PendingQA || status == VacancyStatus.ReservedForQA || 
                   status == VacancyStatus.RejectedByQA;
        }

        public static bool IsStateInQa(this VacancyStatus status)
        {
            return status == VacancyStatus.PendingQA || status == VacancyStatus.ReservedForQA;
        }

        public static bool CanHaveApplicationsOrClickThroughs(this VacancyStatus status)
        {
            return status == VacancyStatus.Live || status == VacancyStatus.Closed ||
                   status == VacancyStatus.Completed || status == VacancyStatus.Withdrawn;
        }

        public static bool CanShowVacancyReferencenumber(this VacancyStatus status)
        {
            return status == VacancyStatus.Live || status == VacancyStatus.Closed ||
                   status == VacancyStatus.Completed || status == VacancyStatus.Withdrawn;
        }

        public static bool CanManageVacancyDates(this VacancyStatus status)
        {
            return status == VacancyStatus.Live || status == VacancyStatus.Closed;
        }

        public static bool CanWithdrawVacancy(this VacancyStatus status)
        {
            return status == VacancyStatus.Live;
        }
    }
}