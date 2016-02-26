namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Common.ViewModels.VacancySearch;
    using ViewModels.VacancySearch;

    public interface ITraineeshipVacancyProvider
    {
        TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search);

        TraineeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);
    }
}