using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipSearchMediator
    {
        MediatorResponse<TraineeshipSearchViewModel> Index(Guid? candidateId);

        MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model);

        MediatorResponse<TraineeshipVacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId);

        MediatorResponse<TraineeshipVacancyDetailViewModel> DetailsByReferenceNumber(string vacancyReferenceNumberString, Guid? candidateId);

        MediatorResponse<TraineeshipSearchViewModel> SearchValidation(TraineeshipSearchViewModel model);
    }
}