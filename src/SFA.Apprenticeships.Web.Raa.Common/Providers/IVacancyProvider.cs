namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using ViewModels;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;

    public interface IVacancyProvider
    {
		List<VacancyViewModel> GetVacanciesForProvider(string ukprn, string providerSiteErn);

        VacanciesSummaryViewModel GetVacanciesSummaryForProvider(string ukprn, string providerSiteErn, VacanciesSummarySearchViewModel vacanciesSummarySearch);
	
        List<DashboardVacancySummaryViewModel> GetPendingQAVacanciesOverview();

        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);

        void RejectVacancy(long vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber);

        VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel);

        NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);
    }
}
