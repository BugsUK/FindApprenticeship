namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(string username);

        NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);

        List<SectorSelectItemViewModel> GetSectorsAndFrameworks();

        List<SelectListItem> GetApprenticeshipLevels();
    }
}
