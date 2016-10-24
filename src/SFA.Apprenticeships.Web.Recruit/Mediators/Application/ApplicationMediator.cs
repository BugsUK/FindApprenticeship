namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Security;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Raa.Common.Validators.VacancyStatus;
    using Raa.Common.ViewModels.Application;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ApplicationMediator : MediatorBase, IApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ShareApplicationsViewModelValidator _shareApplicationsViewModelValidator;
        private readonly IEncryptionService<AnonymisedApplicationLink> _encryptionService;
        private readonly IDateTimeService _dateTimeService;
        private readonly BulkApplicationsRejectViewModelServerValidator _bulkApplicationsRejectViewModelServerValidator;

        public ApplicationMediator(IApplicationProvider applicationProvider,
            ShareApplicationsViewModelValidator shareApplicationsViewModelValidator,
            IEncryptionService<AnonymisedApplicationLink> encryptionService, IDateTimeService dateTimeService,
            BulkApplicationsRejectViewModelServerValidator bulkApplicationsRejectViewModelServerValidator)
        {
            _applicationProvider = applicationProvider;
            _shareApplicationsViewModelValidator = shareApplicationsViewModelValidator;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
            _bulkApplicationsRejectViewModelServerValidator = bulkApplicationsRejectViewModelServerValidator;
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
                var anonymisedApplicationLinkData = new AnonymisedApplicationLink(application.ApplicationId, _dateTimeService.TwoWeeksFromUtcNow);
                var encryptedLinkData = _encryptionService.Encrypt(anonymisedApplicationLinkData);
                var urlEncodedLinkData = HttpUtility.UrlEncode(encryptedLinkData);
                var routeName = newViewModel.VacancyType == VacancyType.Apprenticeship ? RecruitmentRouteNames.ViewAnonymousApprenticeshipApplication : RecruitmentRouteNames.ViewAnonymousTraineeshipApplication;
                var routeValues = new RouteValueDictionary();
                routeValues["application"] = urlEncodedLinkData;
                var link = urlHelper.RouteUrl(routeName, routeValues);
                applicationLinks[application.ApplicantID] = link;
            }

            _applicationProvider.ShareApplications(viewModel.VacancyReferenceNumber, newViewModel.ProviderName, applicationLinks, _dateTimeService.TwoWeeksFromUtcNow, viewModel.RecipientEmailAddress);

            return GetMediatorResponse(ApplicationMediatorCodes.ShareApplications.Ok, newViewModel);
        }

        public MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _applicationProvider.GetBulkDeclineCandidatesViewModel(vacancyReferenceNumber);
            return new MediatorResponse<BulkDeclineCandidatesViewModel>
            {
                ViewModel = model,
                Code = ApprenticeshipApplicationMediatorCodes.BulkDeclineCandidatesViewModel.Ok,
            };
        }

        public MediatorResponse<BulkDeclineCandidatesViewModel> BulkResponseApplications(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel)
        {
            BulkDeclineCandidatesViewModel viewModel = _applicationProvider.GetBulkDeclineCandidatesViewModel(bulkApplicationsRejectViewModel.VacancyReferenceNumber);
            viewModel.SelectedApplicationIds = bulkApplicationsRejectViewModel.ApplicationIds;
            var validationResult = _bulkApplicationsRejectViewModelServerValidator.Validate(bulkApplicationsRejectViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.FailedValidation, viewModel, validationResult);
            }
            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok, viewModel);
        }

        public MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearchViewModel)
        {
            var viewModel = _applicationProvider.GetBulkDeclineCandidatesViewModel(vacancyApplicationsSearchViewModel);
            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.GetBulkDeclineCandidatesViewModel.Ok, viewModel);
        }

        public BulkApplicationsRejectViewModel GetApplicationViewModel(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel)
        {
            return _applicationProvider.GetBulkApplicationsRejectViewModel(bulkApplicationsRejectViewModel);
        }
    }
}