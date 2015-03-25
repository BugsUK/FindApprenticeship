namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;
    using ViewModels.Account;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchViewModel> Index(Guid? candidateId, ApprenticeshipSearchMode searchMode);

        MediatorResponse<ApprenticeshipSearchViewModel> SearchValidation(Guid? candidateId, ApprenticeshipSearchViewModel model);

        MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(Guid? candidateId, ApprenticeshipSearchViewModel model);

        MediatorResponse<ApprenticeshipSearchViewModel> SaveSearch(Guid candidateId, ApprenticeshipSearchViewModel viewModel);

        MediatorResponse<ApprenticeshipVacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId);

        MediatorResponse<SavedSearchViewModel> RunSavedSearch(Guid candidateId, ApprenticeshipSearchViewModel apprenticeshipSearchViewModel);
    }
}
