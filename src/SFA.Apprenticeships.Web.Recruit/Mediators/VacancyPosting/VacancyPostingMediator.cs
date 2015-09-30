namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using Providers;
    using ViewModels.Vacancy;

    public class VacancyPostingMediator : IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;

        public VacancyPostingMediator(IVacancyPostingProvider vacancyPostingProvider)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
        }

        public NewVacancyViewModel Index(string username)
        {
            return _vacancyPostingProvider.GetNewVacancy(username);
        }
    }
}
