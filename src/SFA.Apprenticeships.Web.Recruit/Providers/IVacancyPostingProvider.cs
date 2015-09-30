namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.Vacancy;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancy(string username);
    }
}
