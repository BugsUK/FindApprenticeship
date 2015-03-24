namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.VacancySearch;

    public interface IApprenticeshipVacancyProvider
    {
        ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search); 
    }
}