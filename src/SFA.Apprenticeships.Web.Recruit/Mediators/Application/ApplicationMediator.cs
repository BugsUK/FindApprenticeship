namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Raa.Common.ViewModels.Application;

    public class ApplicationMediator : MediatorBase, IApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ShareApplicationsViewModelValidator _shareApplicationsViewModelValidator;

        public ApplicationMediator(IApplicationProvider applicationProvider, ShareApplicationsViewModelValidator shareApplicationsViewModelValidator)
        {
            _applicationProvider = applicationProvider;
            _shareApplicationsViewModelValidator = shareApplicationsViewModelValidator;
        }

        public MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var viewModel = _applicationProvider.GetVacancyApplicationsViewModel(vacancyApplicationsSearch);

            return GetMediatorResponse(ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok, viewModel);
        }

        public MediatorResponse<ShareApplicationsViewModel> ShareApplications(int vacancyReferenceNumber)
        {
            var viewModel = _applicationProvider.GetShareApplicationsViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(ApplicationMediatorCodes.GetShareApplicationsViewModel.Ok, viewModel);
        }

        public MediatorResponse<ShareApplicationsViewModel> ShareApplications(ShareApplicationsViewModel viewModel)
        {
            var validationResult = _shareApplicationsViewModelValidator.Validate(viewModel);

            var newViewModel = _applicationProvider.GetShareApplicationsViewModel(viewModel.VacancyReferenceNumber);
            newViewModel.SelectedApplicationIds = viewModel.SelectedApplicationIds;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApplicationMediatorCodes.ShareApplications.FailedValidation, newViewModel, validationResult);
            }

            return GetMediatorResponse(ApplicationMediatorCodes.ShareApplications.Ok, newViewModel);
        }
    }
}