namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.Vacancy;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(string username);

        VacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);
    }
}
