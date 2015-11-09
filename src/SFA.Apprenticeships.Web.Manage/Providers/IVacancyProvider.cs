namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Collections.Generic;
    using ViewModels;
    using Raa.Common.ViewModels.Vacancy;

    public interface IVacancyProvider
    {
        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);

        void RejectVacancy(long vacancyReferenceNumber);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);
    }
}