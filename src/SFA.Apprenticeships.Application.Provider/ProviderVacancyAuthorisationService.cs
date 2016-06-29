namespace SFA.Apprenticeships.Application.Provider
{
    using System;
    using System.Linq;
    using Domain.Entities.Raa;
    using Infrastructure.Interfaces;
    using Interfaces.Providers;

    public class ProviderVacancyAuthorisationService : IProviderVacancyAuthorisationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderService _providerService;

        public ProviderVacancyAuthorisationService(ICurrentUserService currentUserService, IProviderService providerService)
        {
            _currentUserService = currentUserService;
            _providerService = providerService;
        }

        public void Authorise(int providerId, int? providerSiteId)
        {
            if (!_currentUserService.IsInRole(Roles.Faa))
            {
                // Only Provider Users require authorisation.
                return;
            }

            var signedInProviderId = Convert.ToInt32(_currentUserService.GetClaimValue("providerId"));

            if (providerId == signedInProviderId)
            {
                return;
            }

            //Fall back to Provider Site Id as the assigned provider for a vacancy could be a sub contractor
            if (providerSiteId.HasValue)
            {
                var providerSites = _providerService.GetProviderSites(signedInProviderId);
                if (providerSites.Any(ps => ps.ProviderSiteId == providerSiteId))
                {
                    return;
                }
            }

            var message = $"Provider user '{_currentUserService.CurrentUserName}' (signed in as Provider Id {signedInProviderId}) attempted to view vacancy for Provider Id {providerId} and Provider Site Id {providerSiteId}";

            throw new Domain.Entities.Exceptions.CustomException(message, Interfaces.ErrorCodes.ProviderVacancyAuthorisationFailed);
        }
    }

    public class NullVacancyAuthorisationService : IProviderVacancyAuthorisationService
    {
        //TODO: Remove once claims issue is resolved
        public void Authorise(int providerId, int? providerSiteId)
        {
            //Always authorise
        }
    }
}
