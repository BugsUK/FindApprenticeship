namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apprenticeships.Application.Interfaces.Security;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Raa.Common.ViewModels.Application;
    using SFA.Infrastructure.Interfaces;

    public class ApplicationMediator : MediatorBase, IApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ShareApplicationsViewModelValidator _shareApplicationsViewModelValidator;
        private readonly IEncryptionService<AnonymisedApplicationLink> _encryptionService;
        private readonly IDateTimeService _dateTimeService;

        public ApplicationMediator(IApplicationProvider applicationProvider, ShareApplicationsViewModelValidator shareApplicationsViewModelValidator, IEncryptionService<AnonymisedApplicationLink> encryptionService, IDateTimeService dateTimeService)
        {
            _applicationProvider = applicationProvider;
            _shareApplicationsViewModelValidator = shareApplicationsViewModelValidator;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
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

        public MediatorResponse<ShareApplicationsViewModel> ShareApplications(ShareApplicationsViewModel viewModel, UrlHelper urlHelper)
        {
            var validationResult = _shareApplicationsViewModelValidator.Validate(viewModel);

            var newViewModel = _applicationProvider.GetShareApplicationsViewModel(viewModel.VacancyReferenceNumber);
            newViewModel.SelectedApplicationIds = viewModel.SelectedApplicationIds;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApplicationMediatorCodes.ShareApplications.FailedValidation, newViewModel, validationResult);
            }

            var applicationLinks = new Dictionary<string, string>();
            foreach (var selectedApplicationId in viewModel.SelectedApplicationIds)
            {
                var application = newViewModel.ApplicationSummaries.Single(a => a.ApplicationId == selectedApplicationId);
                var anonymisedApplicationLink = new AnonymisedApplicationLink(application.ApplicationId, _dateTimeService.TwoWeeksFromUtcNow);
                var encryptedLink = _encryptionService.Encrypt(anonymisedApplicationLink);
                var routeName = newViewModel.VacancyType == VacancyType.Apprenticeship ? RecruitmentRouteNames.ViewAnonymousApprenticeshipApplication : RecruitmentRouteNames.ViewAnonymousTraineeshipApplication;
                var link = urlHelper.RouteUrl(routeName, new { application = encryptedLink });
                applicationLinks[application.ApplicantID] = link;
            }

            _applicationProvider.ShareApplications(viewModel.VacancyReferenceNumber, newViewModel.EmployerName, applicationLinks, _dateTimeService.TwoWeeksFromUtcNow, viewModel.RecipientEmailAddress);

            return GetMediatorResponse(ApplicationMediatorCodes.ShareApplications.Ok, newViewModel);
        }
    }
}