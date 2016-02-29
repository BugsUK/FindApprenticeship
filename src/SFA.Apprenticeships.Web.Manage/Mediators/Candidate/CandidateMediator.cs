namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using System;
    using Application.Interfaces.Applications;
    using Common.Mediators;
    using Providers;
    using Raa.Common.Factories;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.Application.Traineeship;
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

        public MediatorResponse<CandidateApplicationsViewModel> GetCandidateApplications(Guid candidateId)
        {
            var viewModel = _candidateProvider.GetCandidateApplications(candidateId);

            return GetMediatorResponse(CandidateMediatorCodes.GetCandidateApplications.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> GetCandidateApprenticeshipApplication(Guid applicationId)
        {
            var viewModel = _candidateProvider.GetCandidateApprenticeshipApplication(applicationId);

            return GetMediatorResponse(CandidateMediatorCodes.GetCandidateApprenticeshipApplication.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> GetCandidateTraineeshipApplication(Guid applicationId)
        {
            var viewModel = _candidateProvider.GetCandidateTraineeshipApplication(applicationId);

            return GetMediatorResponse(CandidateMediatorCodes.GetCandidateTraineeshipApplication.Ok, viewModel);
        }
    }
}