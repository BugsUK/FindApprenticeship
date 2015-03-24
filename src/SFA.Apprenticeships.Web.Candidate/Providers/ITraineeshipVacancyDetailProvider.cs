namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipVacancyDetailProvider
    {
        TraineeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);
    }
}
