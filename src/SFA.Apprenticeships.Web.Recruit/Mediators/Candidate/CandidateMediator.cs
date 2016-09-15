namespace SFA.Apprenticeships.Web.Recruit.Mediators.Candidate
{
    using Common.Mediators;
    using Raa.Common.Factories;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Candidate;
    using Raa.Common.ViewModels.Candidate;

    public class CandidateMediator : MediatorBase, ICandidateMediator
    {
        private readonly ICandidateProvider _candidateProvider;

        private readonly CandidateSearchViewModelServerValidator _candidateSearchViewModelServerValidator;

        public CandidateMediator(ICandidateProvider candidateProvider, CandidateSearchViewModelServerValidator candidateSearchViewModelServerValidator)
        {
            _candidateProvider = candidateProvider;
            _candidateSearchViewModelServerValidator = candidateSearchViewModelServerValidator;
        }

        public MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel searchViewModel)
        {
            searchViewModel.PageSizes = SelectListItemsFactory.GetPageSizes(searchViewModel.PageSize);

            var validatonResult = _candidateSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(CandidateMediatorCodes.Search.FailedValidation, new CandidateSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var resultsViewModel = _candidateProvider.SearchCandidates(searchViewModel);

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, resultsViewModel);
        }

        public MediatorResponse<CandidateApplicationSummariesViewModel> GetCandidateApplications(CandidateApplicationsSearchViewModel searchViewModel)
        {
            var viewModel = _candidateProvider.GetCandidateApplicationSummaries(searchViewModel);

            return GetMediatorResponse(CandidateMediatorCodes.GetCandidateApplications.Ok, viewModel);
        }
    }
}