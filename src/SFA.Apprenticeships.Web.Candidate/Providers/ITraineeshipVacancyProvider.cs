namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipVacancyProvider
    {
        TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search);

        TraineeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);

        TraineeshipVacancyDetailViewModel GetVacancyDetailViewModelByReferenceNumber(Guid? candidateId, int vacancyReferenceNumber);
    }
}