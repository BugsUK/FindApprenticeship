namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Constants;
    using Common.Extensions;
    using Common.Framework;
    using Common.Mediators;
    using Common.Models.Azure.AccessControlService;
    using Common.Providers;
    using Common.Providers.Azure.AccessControlService;
    using Constants.Messages;
    using Providers;
    using Raa.Common.Configuration;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ViewModels;

    public class AgencyUserMediator : MediatorBase, IAgencyUserMediator
    {
        private readonly IAgencyUserProvider _agencyUserProvider;
        private readonly IAuthorizationErrorProvider _authorizationErrorProvider;
        private readonly IConfigurationService _configurationService;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IVacancyQAProvider _vacancyQaProvider;

        public AgencyUserMediator(IAgencyUserProvider agencyUserProvider,
            IAuthorizationErrorProvider authorizationErrorProvider, IUserDataProvider userDataProvider,
            IVacancyQAProvider vacancyQaProvider, IConfigurationService configurationService)
        {
            _agencyUserProvider = agencyUserProvider;
            _authorizationErrorProvider = authorizationErrorProvider;
            _userDataProvider = userDataProvider;
            _vacancyQaProvider = vacancyQaProvider;
            _configurationService = configurationService;
        }

        public MediatorResponse<AgencyUserViewModel> Authorize(ClaimsPrincipal principal)
        {
            var viewModel = new AgencyUserViewModel();

            if (string.IsNullOrEmpty(principal?.Identity?.Name))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.EmptyUsername, viewModel,
                    AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
            }

            var authorisationGroupClaim = _configurationService.Get<ManageWebConfiguration>().AuthorisationGroupClaim;

            if (!principal.IsInGroup(authorisationGroupClaim))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.MissingServicePermission, viewModel,
                    AuthorizeMessages.MissingServicePermission, UserMessageLevel.Error);
            }

            var username = principal.Identity.Name;
            viewModel = _agencyUserProvider.GetOrCreateAgencyUser(username);

            // Redirect to session return URL (if any).
            var returnUrl = _userDataProvider.Pop(UserDataItemNames.ReturnUrl);
            if (returnUrl.IsValidReturnUrl())
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.ReturnUrl, viewModel, parameters: returnUrl);
            }

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }

        public AuthorizationErrorDetailsViewModel AuthorizationError(string errorDetails)
        {
            return _authorizationErrorProvider.GetAuthorizationErrorDetailsViewModel(errorDetails);
        }

        public MediatorResponse<AgencyUserViewModel> GetAgencyUser(ClaimsPrincipal principal)
        {
            var username = principal.Identity.Name;
            var viewModel = _agencyUserProvider.GetAgencyUser(username);

            return GetMediatorResponse(AgencyUserMediatorCodes.GetAgencyUser.Ok, viewModel);
        }

        public MediatorResponse<AgencyUserViewModel> SaveAgencyUser(ClaimsPrincipal principal,
            AgencyUserViewModel viewModel)
        {
            var username = principal.Identity.Name;
            viewModel = _agencyUserProvider.SaveAgencyUser(username, viewModel);

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }

        public MediatorResponse<HomeViewModel> GetHomeViewModel(ClaimsPrincipal principal,
            DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            var username = principal.Identity.Name;
            var userViewModel = _agencyUserProvider.GetAgencyUser(username);
            var vacancySummariesViewModel = _vacancyQaProvider.GetPendingQAVacanciesOverview(searchViewModel);

            var homeViewModel = new HomeViewModel
            {
                AgencyUser = userViewModel,
                VacancySummaries = vacancySummariesViewModel
            };

            return GetMediatorResponse(AgencyUserMediatorCodes.GetHomeViewModel.OK, homeViewModel);
        }
    }
}