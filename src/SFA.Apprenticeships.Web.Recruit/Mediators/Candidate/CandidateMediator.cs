namespace SFA.Apprenticeships.Web.Recruit.Mediators.Candidate
{
    using System;
    using System.Collections.Generic;
    using Common.Mediators;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Raa.Common.Factories;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Candidate;
    using Raa.Common.ViewModels.Application;
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

        public MediatorResponse<CandidateApplicationSummariesViewModel> GetCandidateApplications(Guid candidateGuid)
        {
            var viewModel = new CandidateApplicationSummariesViewModel
            {
                CandidateApplicationsSearch = new CandidateApplicationsSearchViewModel(candidateGuid),
                ApplicantDetails = new ApplicantDetailsViewModel {Address = new AddressViewModel()},
                ApplicationSummaries = new PageableViewModel<CandidateApplicationSummaryViewModel> { Page = new List<CandidateApplicationSummaryViewModel>()}
            };

            return GetMediatorResponse(CandidateMediatorCodes.GetCandidateApplications.Ok, viewModel);
        }
    }
}