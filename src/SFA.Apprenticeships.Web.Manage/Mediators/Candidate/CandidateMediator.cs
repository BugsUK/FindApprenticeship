namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using Common.Mediators;
    using Providers;
    using Validators;
    using ViewModels;

    public class CandidateMediator : MediatorBase, ICandidateMediator
    {
        private readonly ICandidateProvider _candidateProvider;
        private readonly CandidateSearchResultsViewModelServerValidator _candidateSearchResultsViewModelServerValidator;

        public CandidateMediator(ICandidateProvider candidateProvider, CandidateSearchResultsViewModelServerValidator candidateSearchResultsViewModelServerValidator)
        {
            _candidateProvider = candidateProvider;
            _candidateSearchResultsViewModelServerValidator = candidateSearchResultsViewModelServerValidator;
        }

        public MediatorResponse<CandidateSearchResultsViewModel> Search()
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel()
            };

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, viewModel);
        }

        public MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchResultsViewModel viewModel)
        {
            var validatonResult = _candidateSearchResultsViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(CandidateMediatorCodes.Search.FailedValidation, viewModel, validatonResult);
            }

            var resultsViewModel = _candidateProvider.SearchCandidates(viewModel.SearchViewModel);

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, resultsViewModel);
        }
    }
}