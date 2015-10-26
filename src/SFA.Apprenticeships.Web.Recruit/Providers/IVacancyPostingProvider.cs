namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern);

        NewVacancyViewModel GetNewVacancyViewModel(long vacancyReferenceNumber);

        NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        VacancySummaryViewModel GetVacancySummaryViewModel(long vacancyReferenceNumber);

        VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);

        VacancyViewModel SubmitVacancy(VacancyViewModel viewModel);

        List<SectorSelectItemViewModel> GetSectorsAndFrameworks();

        List<SelectListItem> GetApprenticeshipLevels();
    }
}
