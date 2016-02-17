namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using Common.Mediators;
    using Providers;
    using Raa.Common.Factories;
    using Validators;
    using ViewModels;

    public class CandidateMediator : MediatorBase, ICandidateMediator
    {
        private readonly ICandidateProvider _candidateProvider;
        private readonly CandidateSearchViewModelServerValidator _candidateSearchViewModelServerValidator;

        public CandidateMediator(ICandidateProvider candidateProvider, CandidateSearchViewModelServerValidator candidateSearchViewModelServerValidator)
        {
            _candidateProvider = candidateProvider;
            _candidateSearchViewModelServerValidator = candidateSearchViewModelServerValidator;
        }

        public MediatorResponse<CandidateSearchResultsViewModel> Search()
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel()
            };

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, viewModel);
        }

        public MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel viewModel)
        {
            viewModel.PageSizes = SelectListItemsFactory.GetPageSizes(viewModel.PageSize);

            var validatonResult = _candidateSearchViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(CandidateMediatorCodes.Search.FailedValidation, new CandidateSearchResultsViewModel { SearchViewModel = viewModel }, validatonResult);
            }

            var resultsViewModel = _candidateProvider.SearchCandidates(viewModel);

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, resultsViewModel);
        }
    }
}