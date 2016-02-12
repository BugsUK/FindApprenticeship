namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using Common.Mediators;
    using Validators;
    using ViewModels;

    public class CandidateMediator : MediatorBase, ICandidateMediator
    {
        private readonly CandidateSearchResultsViewModelServerValidator _candidateSearchResultsViewModelServerValidator;

        public CandidateMediator(CandidateSearchResultsViewModelServerValidator candidateSearchResultsViewModelServerValidator)
        {
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

            return GetMediatorResponse(CandidateMediatorCodes.Search.Ok, viewModel);
        }
    }
}