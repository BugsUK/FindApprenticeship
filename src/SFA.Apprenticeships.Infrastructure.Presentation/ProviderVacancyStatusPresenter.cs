namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Raa.Vacancies;

    public static class ProviderVacancyStatusPresenter
    {
        public static bool IsStateReadOnly(this VacancyStatus status)
        {
            return status == VacancyStatus.Submitted || status == VacancyStatus.Live ||
                   status == VacancyStatus.ReservedForQA || status == VacancyStatus.Closed ||
                   status == VacancyStatus.Withdrawn || status == VacancyStatus.Completed ||
                   status == VacancyStatus.PostedInError;
        }

        public static bool IsStateEditable(this VacancyStatus status)
        {
            return status == VacancyStatus.Unknown || status == VacancyStatus.Draft ||
                   status == VacancyStatus.Referred;
        }

        public static bool IsStateReviewable(this VacancyStatus status)
        {
            return status == VacancyStatus.Submitted || status == VacancyStatus.ReservedForQA ||
                   status == VacancyStatus.Referred;
        }

        public static bool IsStateInQa(this VacancyStatus status)
        {
            return status == VacancyStatus.Submitted || status == VacancyStatus.ReservedForQA;
        }

        public static bool IsStateEditablePostLive(this VacancyStatus status)
        {
            return status == VacancyStatus.Submitted || status == VacancyStatus.ReservedForQA;
        }

        public static bool IsStateDeletable(this VacancyStatus status)
        {
            return status == VacancyStatus.Draft || status == VacancyStatus.Referred;
        }

        public static bool CanHaveApplicationsOrClickThroughs(this VacancyStatus status)
        {
            return status == VacancyStatus.Live || status == VacancyStatus.Closed ||
                   status == VacancyStatus.Completed || status == VacancyStatus.Withdrawn;
        }

        public static bool CanManageVacancyDates(this VacancyStatus status)
        {
            return status == VacancyStatus.Live || status == VacancyStatus.Closed;
        }

        public static bool CanCloseVacancy(this VacancyStatus status)
        {
            return status == VacancyStatus.Live;
        }

        public static bool CanWithdrawVacancy(this VacancyStatus status)
        {
            return status == VacancyStatus.Live;
        }

        public static bool CanShareApplications(this VacancyStatus status, int totalNumberOfApplications)
        {
            return totalNumberOfApplications > 0 && (status == VacancyStatus.Live || status == VacancyStatus.Closed || status == VacancyStatus.Completed);
        }

        public static bool CanArchiveVacancy(this VacancyStatus status)
        {
            return status == VacancyStatus.Closed;
        }

        public static bool CanBulkDecline(this VacancyStatus status, int totalNumberOfApplications)
        {
            return totalNumberOfApplications > 0 && (status == VacancyStatus.Live || status == VacancyStatus.Closed || status == VacancyStatus.Completed);
        }

        public static bool CanEditWage(this VacancyStatus status, VacancyType vacancyType)
        {
            return vacancyType == VacancyType.Apprenticeship && (status == VacancyStatus.Live || status == VacancyStatus.Closed);
        }
    }
}