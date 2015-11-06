namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Collections.Generic;
    using ViewModels;

    public interface IVacancyProvider
    {
        List<VacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);
    }
}