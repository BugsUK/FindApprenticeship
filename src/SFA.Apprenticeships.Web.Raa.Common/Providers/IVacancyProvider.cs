using System.Collections.Generic;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IVacancyProvider
    {
		List<VacancyViewModel> GetVacanciesForProvider(string ukprn);
	
        List<DashboardVacancySummaryViewModel> GetPendingQAVacanciesOverview();

        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);

        void RejectVacancy(long vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber);
    }
}
