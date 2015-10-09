namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(string providerSiteErn, string ern);

        NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);

        VacancyViewModel SubmitVacancy(VacancyViewModel viewModel);


        List<SectorSelectItemViewModel> GetSectorsAndFrameworks();

        List<SelectListItem> GetApprenticeshipLevels();
    }
}
