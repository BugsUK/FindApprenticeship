namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.VacancySearch;

    public interface ITraineeshipVacancyProvider
    {
        TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search);
    }
}