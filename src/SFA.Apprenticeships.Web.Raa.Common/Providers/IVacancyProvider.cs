namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using ViewModels;
    using ViewModels.Vacancy;

    public interface IVacancyProvider
    {
		List<VacancyViewModel> GetVacanciesForProvider(string ukprn, string providerSiteErn);
	
        List<DashboardVacancySummaryViewModel> GetPendingQAVacanciesOverview();

        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);

        void RejectVacancy(long vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber);

        VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel);

        NewVacancyViewModel UpdateVacancy(NewVacancyViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);
    }
}
