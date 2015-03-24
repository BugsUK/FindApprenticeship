namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchViewModel> Index(ApprenticeshipSearchMode searchMode);

        MediatorResponse<ApprenticeshipSearchViewModel> SearchValidation(ApprenticeshipSearchViewModel model);

        MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(Guid? candidateId, ApprenticeshipSearchViewModel model);

        MediatorResponse<ApprenticeshipSearchViewModel> SaveSearch(Guid candidateId, ApprenticeshipSearchViewModel viewModel);

        MediatorResponse<ApprenticeshipVacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId);
    }
}
